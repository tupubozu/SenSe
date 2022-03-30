# SenSe
Repository for data collection tools for the SenSe-project at HVL

## Usage
    python3 path/to/main.py (MAC address of sensor) (data type(s))
    
Data type flags:
* --ECG
    - Description: Electrocardiogram
* --PPG 
    - Description: Photoplethysmogram
* --ACC
    - Description: Accelerometer
* --PPI
    - Description: Peak-to-Peak interval 
* --GYR
    - Description: Gyroscope
* --MAG
    - Description: Magnetometer

Note: syntax is currently subject to heavy change 
    
### Example
    python3 main.py 00:AA:CC:FF:00:11 --ECG --PPI


