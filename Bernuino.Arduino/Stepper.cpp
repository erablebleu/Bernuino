#include "Stepper.h"

Stepper::Stepper(int limitPin, int dirPin, int stepPin, int stepCount, float lowerValue, float upperValue) {
   _limitPin = limitPin;
   _dirPin = dirPin;
   _stepPin = stepPin;
   _stepCount = stepCount;
   _lowerValue = lowerValue;
   _upperValue = upperValue;
   _isInit = false;

   pinMode(limitPin, INPUT);
   pinMode(dirPin, OUTPUT);
   pinMode(stepPin, OUTPUT);
}

void Stepper::init() {
   digitalWrite(_dirPin, LOW);
   delayMicroseconds(SETUP_TIME);

   while (HIGH == digitalRead(_limitPin))
      step();

   _position = 0;
   _isInit = true;
}


int Stepper::goTo(int val) {

#ifdef DEBUG
   Serial.print("goTo(int)   : ");
   Serial.println(val);
#endif

   return step(val - _position);
}

int Stepper::goTo(float val) {

#ifdef DEBUG
   Serial.print("goTo(float) : ");
   Serial.println(val);
#endif

   return goTo((int)((val - _lowerValue) * _stepCount / (_upperValue - _lowerValue)));
}

void Stepper::step() {
   digitalWrite(_stepPin, HIGH);
   delayMicroseconds(HIGH_PULSE_WIDTH);
   digitalWrite(_stepPin, LOW);
   delayMicroseconds(LOW_PULSE_WIDTH);
}

int Stepper::step(int val) {
   if (_position + val < 0) val = -_position;
   else if (_position + val > _stepCount) val = _stepCount - _position;
   else if (val == 0) return 0;

#ifdef DEBUG
   Serial.print("Step from = ");
   Serial.print(_position);
   Serial.print(" ,val = ");
   Serial.println(val);
#endif

   _position += val;
   digitalWrite(_dirPin, val > 0 ? HIGH : LOW);

   int inc = val > 0 ? val : -val;

   delayMicroseconds(SETUP_TIME);
   for (int i = 0; i < inc; i++)
      step();

   return val;
}