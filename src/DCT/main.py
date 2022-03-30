import asyncio, os, sys, signal
from logging import fatal
from io import TextIOBase
from datetime import datetime, timedelta, timezone
from bleak import BleakClient
from bleak.uuids import uuid16_dict

""" Predefined UUID (Universal Unique Identifier) mapping are based on Heart Rate GATT service Protocol that most
Fitness/Heart Rate device manufacturer follow (Polar H10 in this case) to obtain a specific response input from 
the device acting as an API """

uuid16_dict_reversed = {v: k for k, v in uuid16_dict.items()}

## UUID for model number ##
MODEL_NBR_UUID = "0000{0:x}-0000-1000-8000-00805f9b34fb".format(
    uuid16_dict_reversed.get("Model Number String")
)

## UUID for manufacturer name ##
MANUFACTURER_NAME_UUID = "0000{0:x}-0000-1000-8000-00805f9b34fb".format(
    uuid16_dict_reversed.get("Manufacturer Name String")
)

## UUID for battery level ##
BATTERY_LEVEL_UUID = "0000{0:x}-0000-1000-8000-00805f9b34fb".format(
    uuid16_dict_reversed.get("Battery Level")
)

## UUID for connection establsihment with device ##
PMD_SERVICE = "FB005C80-02E7-F387-1CAD-8ACD2D8DF0C8"

## UUID for Request of stream settings ##
PMD_CONTROL = "FB005C81-02E7-F387-1CAD-8ACD2D8DF0C8"

## UUID for Request of start stream ##
PMD_DATA = "FB005C82-02E7-F387-1CAD-8ACD2D8DF0C8"

## UUID for Request of ECG Stream ##
ECG_WRITE = bytearray([0x02, 0x00, 0x00, 0x01, 0x82, 0x00, 0x01, 0x01, 0x0E, 0x00])

## Command for Request of ECG Stream settings ##
CMD_ECG_STREAM_SETTINGS_READ = bytearray([0x01, 0x00])

## For Polar H10 ECG sampling frequency ##
SAMPLING_FREQ_ECG = 130


ECG_data_collection_flag = False
PPG_data_collection_flag = False
ACC_data_collection_flag = False
PPI_data_collection_flag = False
GYR_data_collection_flag = False
MAG_data_collection_flag = False

ECG_data_matrix = []
PPG_data_matrix = []
ACC_data_matrix = []
PPI_data_matrix = []
GYR_data_matrix = []
MAG_data_matrix = []


class PolarDataCodes:
    # Measurement type codes
    meas_type_dict = { 
        "ECG": 0x00,
        "PPG": 0x01,
        "ACC": 0x02,
        "PPI": 0x03,
        # Code 0x04: Reserved for future use
        "GYR": 0x05,
        "MAG": 0x06,
    }
    
    # Sample frequency codes
    sample_freq_dict = {
        25:  bytearray([0x19,0x00]),
        50:  bytearray([0x32,0x00]),
        52:  bytearray([0x34,0x00]),
        100: bytearray([0x64,0x00]),
        130: bytearray([0x82,0x00]),
        200: bytearray([0xc8,0x00]),
    }
    
    # Sample resolution codes
    sample_res_dict = {
        14: bytearray([0x0e,0x00]),
        16: bytearray([0x10,0x00]),
        22: bytearray([0x16,0x00]),
    }
    
    # Error codes 
    error_dict = {
        0:  "SUCCESS",
        1:  "ERROR: INVALID OP CODE",
        2:  "ERROR: INVALID MEASUREMENT TYPE",
        3:  "ERROR: MEASUREMENT TYPE NOT SUPPORTED BY DEVICE",
        4:  "ERROR: INVALID LENGTH",
        5:  "ERROR: INVALID PARAMETER",
        6:  "ERROR: DEVICE ALREADY IN STATE",
        7:  "ERROR: INVALID RESOLUTION",
        8:  "ERROR: INVALID SAMPLE RATE",
        9:  "ERROR: INVALID RANGE",
        10: "ERROR: INVALID MTU",
        11: "ERROR: INVALID NUMBER OF CHANNELS",
        12: "ERROR: DEVICE IN INVALID STATE",
        13: "ERROR: DEVICE IN CHARGER",
    }
    
    # Operation codes
    op_code_dict = {
        0x01: "Get measurement settings",
        0x02: "Start measurement",
        0x03: "Stop measurement",
    }
    
    def get_meas_type(code: bytearray) -> str:
        return {v: k for k,v in PolarDataCodes.meas_type_dict.items()}[code]
    
    def get_meas_code(type: str) -> int:
        return PolarDataCodes.meas_type_dict[type]
    
    def get_freq_numb(code: bytearray) -> int:
        return {v: k for k,v in PolarDataCodes.sample_freq_dict.items()}[code]
    
    def get_freq_code(numb: int) -> bytearray:
        return PolarDataCodes.sample_freq_dict[numb]
    
    def get_res_numb(code: bytearray) -> int:
        return {v: k for k,v in PolarDataCodes.sample_res_dict.items()}[code]
    
    def get_res_code(numb: int) -> bytearray:
        return PolarDataCodes.sample_res_dict[numb]


class PolarDataStreamSettings(PolarDataCodes):
    def __init__(self, measurement_type: str, sample_freq: int, resolution: int = 16, range: int = 0) -> None:
        self.measurement_type = measurement_type
        self.sample_freq = sample_freq
        self.resolution = resolution
        self.range = range
            
    def cmd_start_array(self) -> bytearray:
        cmd_array = bytearray()
        cmd_array.append(0x02) # Op code: Stream request header (start measurement)
        cmd_array.append(PolarDataCodes.get_meas_code(self.measurement_type)) # measurement type
        if not self.measurement_type == "PPI":
            cmd_array.append(0x00) # Settings: Sample frequency
            cmd_array.append(0x01) # number of 16 bit words assosiated with setting
            cmd_array += PolarDataCodes.get_freq_code(self.sample_freq)
            cmd_array.append(0x01) # Settings: Sample resolution (bits)
            cmd_array.append(0x01) # number of 16 bit words assosiated with setting
            cmd_array += PolarDataCodes.get_res_code(self.resolution)
        
            #if self.measurement_type = "ACC":
            #    pass
           
        return cmd_array
    
    def cmd_stop_array(self) -> bytearray:
        cmd_array = bytearray()
        cmd_array.append(0x03) # Op code: Stream request header (stop measurement)
        cmd_array.append(PolarDataCodes.get_meas_code(self.measurement_type)) # measurement type
        
        return cmd_array
    
## Keyboard Interrupt Handler
ctrl_stopp = False
def keyboardInterrupt_handler(signum, frame) -> None:
    print("Keyboard interrupt received...")
    global ctrl_stopp
    ctrl_stopp = True
    print("----------------Recording stopped------------------------")

def convert_ulong_to_timestamp(raw: int) -> datetime:
    timestamp_base = datetime(2000,1,1, tzinfo=timezone.utc)
    return timestamp_base + timedelta(microseconds=raw/1e3)

def convert_array_to_signed_int(data: bytes, offset: int, length: int) -> int:
    return int.from_bytes(
        bytearray(data[offset : offset + length]), byteorder="little", signed=True,
    )

def convert_to_unsigned_int(data: bytes, offset: int, length: int) -> int:
    return int.from_bytes(
        bytearray(data[offset : offset + length]), byteorder="little", signed=False,
    )

def ECG_parse_msg(data: bytes) -> None:
    global SAMPLING_FREQ_ECG
    print("Measurement: ECG")
    timestamp_raw = convert_to_unsigned_int(data, 1, 8) # nanoseconds since 2000-01-01T00:00:00Z
                
    ecg = []
    step = 3
    samples = data[10:]
    offset = 0
    while offset < len(samples):
        ecg.append(convert_array_to_signed_int(samples, offset, step))
        offset += step
        
    ECG_data_matrix.extend([(ecg[i],convert_ulong_to_timestamp(timestamp_raw - int((len(ecg) - i - 1) * 1e9 / SAMPLING_FREQ_ECG)),timestamp_raw - int((len(ecg) - i - 1) * 1e9 / SAMPLING_FREQ_ECG)) for i in range(len(ecg))])
    return

def ACC_parse_msg(data: bytes) -> None:
    print("Measurement: ACC")
    timestamp_raw = convert_to_unsigned_int(data, 1, 8) # nanoseconds since 2000-01-01T00:00:00Z
    print(f"Timestamp: {convert_ulong_to_timestamp(timestamp_raw).isoformat()}")
    return

def PPG_parse_msg(data: bytes) -> None:
    print("Measurement: PPG")
    timestamp_raw = convert_to_unsigned_int(data, 1, 8) # nanoseconds since 2000-01-01T00:00:00Z
    print(f"Timestamp: {convert_ulong_to_timestamp(timestamp_raw).isoformat()}")
    return

def PPI_parse_msg(data: bytes) -> None:
    print("Measurement: PPI") #"Heart rate [BPM] (int), Peak-to-pear [ms] (int), Error estimate (int), Invalid measurement (bool), Skin contact (bool), Skin contact status reporting supported (bool), Sensor timestamp [raw] (int), Sensor timestamt [parsed, ISO] (datetime)
    timestamp_raw = convert_to_unsigned_int(data, 1, 8) # nanoseconds since 2000-01-01T00:00:00Z
    
    ppi = []
    offset = 0
    step = 6
    samples = data[10:]

    while offset < len(samples):
        bpm = convert_array_to_unsigned_int(samples, offset, 1)
        peak_interval = convert_array_to_unsigned_int(samples, offset + 1, 2)
        error_estimate = convert_array_to_unsigned_int(samples, offset + 3, 2)
        flags = convert_array_to_unsigned_int(samples, offset + 5, 1)
        ecg.append((bpm,peak_interval,error_estimate, flags & 0x01 == 0x01, flags & 0x02 == 0x02, flags & 0x04 == 0x04))
        offset += step

    PPI_data_matrix.extend([(ppi[i][0], ppi[i][1], ppi[i][2], ppi[i][3], ppi[i][4], ppi[i][5], timestamp_raw, convert_ulong_to_timestamp(timestamp_raw)) for i in range(len(ppi))])
    #print(f"Timestamp: {convert_ulong_to_timestamp(timestamp_raw).isoformat()}")
    return

def GYR_parse_msg(data: bytes) -> None:
    print("Measurement: GYR")
    timestamp_raw = convert_to_unsigned_int(data, 1, 8) # nanoseconds since 2000-01-01T00:00:00Z
    print(f"Timestamp: {convert_ulong_to_timestamp(timestamp_raw).isoformat()}")
    return

def MAG_parse_msg(data: bytes) -> None:
    print("Measurement: MAG")
    timestamp_raw = convert_to_unsigned_int(data, 1, 8) # nanoseconds since 2000-01-01T00:00:00Z
    print(f"Timestamp: {convert_ulong_to_timestamp(timestamp_raw).isoformat()}")
    return

## Conversion of the binary data stream
def data_stream_read(sender, data: bytes) -> None:
    print(f"[{datetime.now().isoformat()}] Data packet length: {len(data)}")        
    if data[0] == PolarDataCodes.get_meas_code("ECG"):
        ECG_parse_msg(data)
    elif data[0] == PolarDataCodes.get_meas_code("ACC"):
        ACC_parse_msg(data)
    elif data[0] == PolarDataCodes.get_meas_code("PPG"):
        PPG_parse_msg(data)
    elif data[0] == PolarDataCodes.get_meas_code("PPI"):
        PPI_parse_msg(data)
    elif data[0] == PolarDataCodes.get_meas_code("GYR"):
        GYR_parse_msg(data)
    elif data[0] == PolarDataCodes.get_meas_code("MAG"):
        MAG_parse_msg(data)
    return

## Reader/Parser function for control message data stream. 
def ctrl_msg_reader(sender, data: bytes) -> None:
    if data[0] == 0xf0:
        print (f"""[{datetime.now().isoformat()}] CONTROL POINT MESSAGE: (Packet length {len(data)})
Operation:        {PolarDataCodes.op_code_dict[data[1]]} ({hex(data[1])})
Measurement type: {PolarDataCodes.get_meas_type(data[2])} ({hex(data[2])})
Error code:       {PolarDataCodes.error_dict[data[3]]} ({hex(data[3])})\n""")
        if data[3] == 5: #invalid parameter
            global ctrl_stopp
            ctrl_stopp = True
    return

ECG_file_header_trigger = True        
ECG_sample_cntr = 0
def write_ECG_file(f: TextIOBase) -> None:
    global ECG_data_matrix, ECG_sample_cntr, ECG_file_header_trigger
    if ECG_file_header_trigger:
        f.write(f"\"Sample count\",\"Voltage [µV]\",\"Sensor timestamp [raw]\",\"Sensor timestamp [parsed, ISO]\"\n")
        ECG_file_header_trigger = False
    while len(ECG_data_matrix) > 0:
        f.write(f"\"{ECG_sample_cntr}\",\"{ECG_data_matrix[0][0]}\",\"{ECG_data_matrix[0][2]}\",\"{ECG_data_matrix[0][1].isoformat()}\"\n")
        ECG_sample_cntr += 1
        ECG_data_matrix.pop(0) 
    return

PPG_file_header_trigger = True
PPG_sample_cntr = 0
def write_PPG_file(f: TextIOBase) -> None:
    global PPG_data_matrix, PPG_sample_cntr, PPG_file_header_trigger
    while len(PPG_data_matrix) > 0:
        f.write(f"\n")
        PPG_sample_cntr += 1
        PPG_data_matrix.pop(0) 
    return

ACC_file_header_trigger = True
ACC_sample_cntr = 0
def write_ACC_file(f: TextIOBase) -> None:
    global ACC_data_matrix, ACC_sample_cntr, ACC_file_header_trigger
    while len(ACC_data_matrix) > 0:
        f.write(f"\n")
        ACC_sample_cntr += 1
        ACC_data_matrix.pop(0) 
    return

PPI_file_header_trigger = True
PPI_sample_cntr = 0
def write_PPI_file(f: TextIOBase) -> None:
    global PPI_data_matrix, PPI_sample_cntr, PPI_file_header_trigger
    if PPI_file_header_trigger: 
        f.write(f"\"Sample count\",\"Heart rate [BPM]\",\"Peak-to-Peak [ms]\",\"Error estimate\",\"Invalid measurement\",\"Skin contact\",\"Skin contact status reporting supported\",\"Sensor timestamp [raw]\",\"Sensor timestamt [parsed, ISO]\"\n")
        PPI_file_header_trigger = False
    while len(PPI_data_matrix) > 0:
        f.write(f"\"{PPI_sample_cntr}\",\"{PPI_data_matrix[0][0]}\",\"{PPI_data_matrix[0][1]}\",\"{PPI_data_matrix[0][2]}\",\"{PPI_data_matrix[0][3]}\",\"{PPI_data_matrix[0][4]}\",\"{PPI_data_matrix[0][5]}\",\"{PPI_data_matrix[0][6]}\",\"{PPI_data_matrix[0][7]}\"\n")
        PPI_sample_cntr += 1
        PPI_data_matrix.pop(0) 
    return

GYR_file_header_trigger = True
GYR_sample_cntr = 0
def write_GYR_file(f: TextIOBase) -> None:
    global GYR_data_matrix, GYR_sample_cntr, GYR_file_header_trigger
    while len(GYR_data_matrix) > 0:
        f.write(f"\n")
        GYR_sample_cntr += 1
        GYR_data_matrix.pop(0) 
    return

MAG_file_header_trigger = True
MAG_sample_cntr = 0
def write_MAG_file(f: TextIOBase) -> None:
    global MAG_data_matrix, MAG_sample_cntr, MAG_file_header_trigger
    while len(MAG_data_matrix) > 0:
        f.write(f"\n")
        MAG_sample_cntr += 1
        MAG_data_matrix.pop(0) 
    return

## Aynchronous task to start the data stream for ECG ##
async def run(client: BleakClient, addr: str, debug: bool = False) -> None:

    ## Writing chracterstic description to control point for request of UUID (defined above) ##
    await client.is_connected()
    print("---------Device connected--------------")
    model_number = await client.read_gatt_char(MODEL_NBR_UUID)
    manufacturer_name = await client.read_gatt_char(MANUFACTURER_NAME_UUID)
    battery_level = await client.read_gatt_char(BATTERY_LEVEL_UUID)
    print(f"""Model Number: {''.join(map(chr,model_number))}
Manufacturer Name: {''.join(map(chr,manufacturer_name))}
Battery Level: {int(battery_level[0])}%\n""")
    
    global ECG_data_collection_flag, ACC_data_collection_flag, PPI_data_collection_flag, PPG_data_collection_flag, GYR_data_collection_flag, MAG_data_collection_flag

    ## Creating default "start data stream" command
    if ECG_data_collection_flag:
        stream_ecg_config = PolarDataStreamSettings("ECG", SAMPLING_FREQ_ECG, 14)
        pass
    if ACC_data_collection_flag:
        stream_acc_config = PolarDataStreamSettings("ACC", 100)
        pass
    if PPG_data_collection_flag:
        stream_ppg_config = PolarDataStreamSettings("PPG", SAMPLING_FREQ_ECG, 14)
        pass
    if PPI_data_collection_flag:
        stream_ppi_config = PolarDataStreamSettings("PPI", 100)
        pass
    if GYR_data_collection_flag:
        stream_gyr_config = PolarDataStreamSettings("GYR", SAMPLING_FREQ_ECG, 14)
        pass
    if MAG_data_collection_flag:
        stream_mag_config = PolarDataStreamSettings("MAG", 100)
        pass
    
    att_read = await client.read_gatt_char(PMD_CONTROL)
    
    ## Start control message reader/parser
    #await client.write_gatt_char(PMD_CONTROL, CMD_ECG_STREAM_SETTINGS_READ)
    await client.start_notify(PMD_CONTROL, ctrl_msg_reader)
    await asyncio.sleep(5)
   
    ## Start data stream reader/parser
    await client.start_notify(PMD_DATA, data_stream_read)

    # Send start command(s) to sensor
    if ECG_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_ecg_config.cmd_start_array())
        pass
    if ACC_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_acc_config.cmd_start_array())
        pass
    if PPI_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_ppi_config.cmd_start_array())
        pass
    if PPG_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_ppg_config.cmd_start_array())
        pass
    if GYR_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_gyr_config.cmd_start_array())
        pass
    if MAG_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_mag_config.cmd_start_array())
        pass
   
    
    file_base_name = f"data_{addr.replace(':','')}_{datetime.now().strftime('%Y%m%dT%H%M%S')}"
    if ECG_data_collection_flag:
        file_ECG =  open(f"{file_base_name}.ECG.csv","w")
        pass
    if ACC_data_collection_flag:
        file_ACC =  open(f"{file_base_name}.ACC.csv","w")
        pass
    if PPI_data_collection_flag:
        file_PPI =  open(f"{file_base_name}.PPI.csv","w")
        pass
    if PPG_data_collection_flag:
        file_PPG =  open(f"{file_base_name}.PPG.csv","w")
        pass
    if GYR_data_collection_flag:
        file_GYR =  open(f"{file_base_name}.GYR.csv","w")
        pass
    if MAG_data_collection_flag:
        file_MAG =  open(f"{file_base_name}.MAG.csv","w")
        pass
    
    try:
        global ctrl_stopp
        if ctrl_stopp == False: 
            print("Collecting data...")
            while not ctrl_stopp:
                ## Write collected data every 1 second(s)
                await asyncio.sleep(1)
                if ECG_data_collection_flag:
                    write_ECG_file(file_ECG)
                    pass
                if PPG_data_collection_flag:
                    write_PPG_file(file_PPG)
                    pass
                if ACC_data_collection_flag:
                    write_ACC_file(file_ACC)
                    pass
                if PPI_data_collection_flag:
                    write_PPI_file(file_PPI)
                    pass
                if GYR_data_collection_flag:
                    write_GYR_file(file_GYR)
                    pass
                if MAG_data_collection_flag:
                    write_MAG_file(file_MAG)
                    pass
    except Exception as ex:
        print(ex)
    
    ## Stop the stream once data is collected
    # sending stop stream command
    if ECG_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_ecg_config.cmd_stop_array())
        pass
    if ACC_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_acc_config.cmd_stop_array())
        pass
    if PPI_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_ppi_config.cmd_stop_array())
        pass
    if PPG_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_ppg_config.cmd_stop_array())
        pass
    if GYR_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_gyr_config.cmd_stop_array())
        pass
    if MAG_data_collection_flag:
        await client.write_gatt_char(PMD_CONTROL, stream_mag_config.cmd_stop_array())
        pass

    # waiting for stop command acknowledge
    await asyncio.sleep(2)

    # writing final datapoints and closing file handles
    if ECG_data_collection_flag:
        write_ECG_file(file_ECG)
        file_ECG.close() 
        pass
    if ACC_data_collection_flag:
        write_ACC_file(file_ACC)
        file_ACC.close()
        pass
    if PPI_data_collection_flag:
        write_PPI_file(file_PPI)
        file_PPI.close()
        pass
    if PPG_data_collection_flag:
        write_PPG_file(file_PPG)
        file_PPG.close()
        pass
    if GYR_data_collection_flag:
        write_GYR_file(file_GYR)
        file_GYR.close()
        pass
    if MAG_data_collection_flag:
        write_MAG_file(file_MAG)
        file_MAG.close()
        pass

    ## Stopping local listening services
    await client.stop_notify(PMD_DATA)
    await client.stop_notify(PMD_CONTROL)
    print("Stopping data collection...")
    print("[CLOSED] application closed.")

    sys.exit(0)


async def main(args: list) -> None:
    if len(args) > 0:
        global ECG_data_collection_flag, ACC_data_collection_flag, PPI_data_collection_flag, PPG_data_collection_flag, GYR_data_collection_flag, MAG_data_collection_flag

        ECG_data_collection_flag = "--ECG" in args
        ACC_data_collection_flag = "--ACC" in args
        PPI_data_collection_flag = "--PPI" in args
        PPG_data_collection_flag = "--PPG" in args
        GYR_data_collection_flag = "--GYR" in args
        MAG_data_collection_flag = "--MAG" in args

        flags = (ECG_data_collection_flag, ACC_data_collection_flag, PPI_data_collection_flag, PPG_data_collection_flag, GYR_data_collection_flag, MAG_data_collection_flag)

        if not flags.__contains__(True): 
            print("No data type is set for collection.")
            print("Exiting")
        else:
            try:
                async with BleakClient(args[0]) as client:
                #async with BleakClient(ADDRESS) as client:
                    signal.signal(signal.SIGINT, keyboardInterrupt_handler)
                    tasks = [
                        asyncio.create_task(run(client, args[0], True)), # Use if Python 3.7+
                        #asyncio.ensure_future(run(client, ADDRESS_DICT[args[0]], True)), # else ...
                    ]
    
                    await asyncio.gather(*tasks)
            except Exception as ex:
                print(ex)
    else:
        print("No argument provided")
        print("Exiting")


if __name__ == "__main__":
    os.environ["PYTHONASYNCIODEBUG"] = str(1)
    loop = asyncio.new_event_loop()
    asyncio.set_event_loop(loop)
    loop.run_until_complete(main([sys.argv[i] for i in range(1, len(sys.argv)) if len(sys.argv) > 1]))
