#include "Arduino.h"
#include "CodingWheel.h"

CodingWheel::CodingWheel(Settings* settings, int pin1, int pin2, int pin3) {
   _settings = settings;
   _pin1 = pin1;
   _pin2 = pin2;
   _pin3 = pin3;

   pinMode(_pin1, INPUT);
   pinMode(_pin2, INPUT);
   pinMode(_pin3, INPUT);
}

void CodingWheel::read() {
   _last_c1 = _c1;
   _last_c2 = _c2;
   _last_c3 = _c3;
   
   _c1 = analogRead(_pin1) > _settings->Threshold_CodingWheel;
   _c2 = analogRead(_pin2) > _settings->Threshold_CodingWheel;
   _c3 = analogRead(_pin3) > _settings->Threshold_CodingWheel;

   if (isNewLap()) {
      long tmpTime = _lastRotationTime;
      _lastRotationTime = millis();
      _lastDuration = _lastRotationTime - tmpTime;
   }
}