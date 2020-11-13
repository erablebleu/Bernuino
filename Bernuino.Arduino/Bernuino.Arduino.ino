#include "Settings.h"
#include "Stitch.h"
#include "Stepper.h"
#include "CodingWheel.h"
#include "SerialHelper.h"

#define X_STEP_PIN          2   // Digital Pin
#define Y_STEP_PIN          3   // Digital Pin

#define X_DIRECTION_PIN     5   // Digital Pin
#define Y_DIRECTION_PIN     6   // Digital Pin

#define X_LIMIT_PIN         12  // Digital Pin - Green - SpnEn
#define Y_LIMIT_PIN         13  // Digital Pin - Orange - SpnDir

#define REVERSE_PIN         A3  // Analog In Pin - Yellow - CoolEn

#define CODING_WHEEL_1_PIN  A1  // Analog In Pin - Brown - Hold
#define CODING_WHEEL_2_PIN  A0  // Analog In Pin - Gray - Abort
#define CODING_WHEEL_3_PIN  A2  // Analog In Pin - Blue - Resume

#define SERIAL_RX_PIN       9  // Digital Pin - Green - X+
#define SERIAL_TX_PIN       10 // Digital Pin - Orange - Y+

Settings _settings;
Stitch* _stitch;
//vref 0.300
Stepper StepperX(X_LIMIT_PIN, X_DIRECTION_PIN, X_STEP_PIN, 800, -1, 1);
Stepper StepperY(Y_LIMIT_PIN, Y_DIRECTION_PIN, Y_STEP_PIN, STEP_MODE_16, -1, 1);

//SerialHelper SerialHelper(&_settings, SERIAL_RX_PIN, SERIAL_TX_PIN, 9600);
CodingWheel CodingWheel(&_settings, CODING_WHEEL_1_PIN, CODING_WHEEL_2_PIN, CODING_WHEEL_3_PIN);

/* Machine : 
 *  max 17.5 points / seconde
 *  -> 57.14 ms entre 2 points
 * 
 */

/* Moteur :
 *  41.5x41.5x34
 *  4.8V
 *  0.6A
 *  pas sur : 1.8° -> 200 steps / tour
 */

float _x;
float _y;
bool _xSet;
bool _ySet;

void getCoordinates() {
  _x = _stitch->getX();
  _y = _stitch->getY();
  _xSet = false;
  _ySet = false;
    Serial.println("getCoordinates");
    Serial.println(_x);
    Serial.println(_y);
}

void setup() {
  _settings.XOffset = 0; // X : -1 (AR) to 1 (AV)
  _settings.YOffset = 0; // Y : -1 (Left) to 1 (Right)
  _settings.XScale = 1;  // 0 to 1
  _settings.YScale = 1;  // 0 to 1
  _settings.Threshold_Reverse = 50;
  _settings.Threshold_CodingWheel = 800;
  
  Serial.begin(9600);
  
  StepperX.init();
  StepperY.init();
   
  StepperX.goTo((float)0.0);
  StepperY.goTo((float)0.0);

  _stitch = new Stitch(1, 12);
  //SerialHelper.init();

  getCoordinates();
  
  delay(1000);
}

  
void loop() {  
  CodingWheel.read();

  if(!_xSet && CodingWheel.canMoveX()) {
    StepperX.goTo(_x * _settings.XScale + _settings.XOffset);
    Serial.print("Set X : ");
    Serial.println(_x);
    _xSet = true;
  }
  if(!_ySet && CodingWheel.canMoveY()) {
    StepperY.goTo(_y * _settings.YScale + _settings.YOffset);
    Serial.print("Set Y : ");
    Serial.println(_y);
    _ySet = true;
  }
  if(CodingWheel.isNewLap()) {
    _stitch->incStep(false);  
    getCoordinates();
  
    Serial.println("New Lap");
  }

  /* moveX
   * presetX
   * pique
   * NewLap
   * setY
   */

  
  delay(125);
  
  //SerialHelper.read();
  //SerialHelper.send();

}
