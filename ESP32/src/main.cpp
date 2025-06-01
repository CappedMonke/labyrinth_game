#include <Adafruit_BNO055.h>
#include <Adafruit_DRV2605.h>
#include <Adafruit_Sensor.h>
#include <Arduino.h>
#include <Wire.h>

Adafruit_BNO055 bno = Adafruit_BNO055(55, 0x28);
Adafruit_DRV2605 drv;

bool vibrating = false;

void setup() {
  Serial.begin(115200);
  delay(1000);

  if (!bno.begin()) {
    Serial.println("BNO055 not found!");
    while (1)
      ;
  }
  bno.setExtCrystalUse(true);

  if (!drv.begin()) {
    Serial.println("DRV2605 not found!");
    while (1)
      ;
  }

  drv.selectLibrary(1);
  drv.setMode(DRV2605_MODE_INTTRIG);
}

void loop() {
  sensors_event_t event;
  bno.getEvent(&event);

  float x = event.orientation.x;
  float y = event.orientation.y;
  float z = event.orientation.z;

  Serial.print("X: ");
  Serial.print(x);
  Serial.print(" Y: ");
  Serial.print(y);
  Serial.print(" Z: ");
  Serial.println(z);

  if (z > 45) {
    if (!vibrating) {
      // Start vibration
      drv.setWaveform(0, 1); // Strong click
      drv.setWaveform(1, 0); // End sequence
      drv.go();
      vibrating = true;
      Serial.println("VIBRATING...");
    } else {
      // Optional: retrigger every 1s to simulate continuous buzz
      static unsigned long lastTrigger = 0;
      if (millis() - lastTrigger > 100) {
        drv.go();
        lastTrigger = millis();
      }
    }
  } else {
    if (vibrating) {
      // Stop vibration (DRV2605 doesn’t have true stop, so we just stop
      // triggering)
      Serial.println("Stopped vibrating.");
      vibrating = false;
    }
  }

  delay(100);
}
