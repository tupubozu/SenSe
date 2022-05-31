#include <SPI.h>
#include <WiFiNINA.h>
#include <ArduinoBLE.h>
#include <SD.h>

#if defined(ARDUINO_ARCH_AVR)
#include <Arduino_FreeRTOS.h>
#elif defined(ARDUINO_ARCH_SAM)
#include <FreeRTOS_SAMD21.h>
#endif

void setup()
{
    Serial.begin(115200);
    while(!Serial);

    if (!BLE.begin()) {
        Serial.println("Initialization of Bluetooth® Low Energy module failed!");
        // Halt system
        while (true);
    }

    if (WiFi.status() == WL_NO_MODULE) {
        Serial.println("Communication with WiFi module failed!");
        // Halt system
        while (true);
    }

    BLE.scan();
}

void loop()
{
    // change to parameters read from SD card 
    static char ssid[] = "";    // your network SSID (name)
    static char pass[] = "";    // your network password (use for WPA, or use as key for WEP)
    
    static int status = WL_IDLE_STATUS;     // the WiFi radio's status

    while (status != WL_CONNECTED) {
        Serial.print("Attempting to connect to WPA SSID: ");
        Serial.println(ssid);
        // Connect to WPA/WPA2 network:
        status = WiFi.begin(ssid, pass);

        // TODO: Change to polling strategy
        // wait 10 seconds for connection:
        delay(10000);
    }
    
}
