#include <Arduino.h>

void setup() {
  Serial.begin(115200); // Start serial communication
  while (!Serial)
    ; // Wait for serial to be ready (sometimes optional on ESP32)
  Serial.println("ESP32 is up and running!");
}

void loop() {
  Serial.println("Hello from ESP32!");
  delay(1000); // Wait for 1 second
}
