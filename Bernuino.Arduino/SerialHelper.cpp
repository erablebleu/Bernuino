#include "SerialHelper.h"

SerialHelper* _serialHelper;

static void _handleRxChar(uint8_t c) { _serialHelper->handleRxChar(c); }

SerialHelper::SerialHelper(Settings* settings, int rxPin, int txPin, int baud) {
   _serialHelper = this;
   _rxPin = rxPin;
   _txPin = txPin;
   _baud = baud;
   _serial = new NeoSWSerial(rxPin, txPin);
   _settings = settings;
}

void SerialHelper::init() {

   Serial.println("HC-06 | INITIALIZING BLUETOOTH");

   _serial->begin(_baud);
   _serial->attachInterrupt(_handleRxChar);

   Serial.println("HC-06 | SEND CONFIGURATION");

   // _serial->write("AT+NAMEBernuino");  // Set Bluetooth Name : Bernuino
   // delay(1000);
   // while (_serial->available()) Serial.write(_serial->read());
   // _serial->write("AT+BAUD4");         // Set Baud rate      : 9600
   // delay(1000);
   // while (_serial->available()) Serial.write(_serial->read());
   // _serial->write("AT+PIN8937");       // Set PIN            : 8937
   // delay(1000);
   // while (_serial->available()) Serial.write(_serial->read());
}

void SerialHelper::handleRxChar(uint8_t c) {
   if (_dataIdx > 0) {
      if (_dataIdx == 1) _dataSize = c;
      else if (_dataIdx == 2) _dataSize += c * 256;
      else if (_dataIdx == 3) {
         _chkValue = c;
         _chkCalc = 0;
         _data = (byte*)malloc(_dataSize);
      }
      else if (_dataIdx >= _dataSize + 4) {


         Serial.print("Fin réception msg : Size=");
         Serial.print(_dataSize);
         Serial.print(", Chk=");
         Serial.print(_chkValue);
         Serial.print(", Chk_calc=");
         Serial.print(_chkCalc);
         Serial.print(", _dataIdx=");
         Serial.print(_dataIdx);
         Serial.print(", last=");
         Serial.println(c);


         if (c == ETX && _chkCalc == _chkValue && _dataIdx == _dataSize + 4) manageMsg(_data, _dataSize);
         else {
            Serial.println("    => ERROR");

            // Send ErrorMsg
            MSG_TYPE answer = MSG_TYPE_ERROR;
            //write((byte*)&answer, 2);
         }

         if (_data != 0) {
            free(_data);
            _data = 0;
         }

         _dataIdx = -1;
      }
      else {
         _data[_dataIdx - 4] = c;
         _chkCalc += c;
      }

      _dataIdx++;
   }
   else if (c == STX) {
      _dataIdx = 1;
   }
}

void SerialHelper::manageMsg(byte* data, int size) {

   MSG* msg = (MSG*)data;
   MSG* answer;

   switch (msg->MsgType) {
   case MSG_TYPE_SETTING_AFFECTATION:
      switch (msg->SettingAffectation.DataType) {
      case DATA_TYPE_X_OFFSET: _settings->XOffset = msg->SettingAffectation.Value; break;
      case DATA_TYPE_Y_OFFSET: _settings->YOffset = msg->SettingAffectation.Value; break;
      case DATA_TYPE_X_SCALE: _settings->XScale = msg->SettingAffectation.Value; break;
      case DATA_TYPE_Y_SCALE: _settings->YScale = msg->SettingAffectation.Value; break;
      }
      break;
   case MSG_TYPE_SETTING_REQUEST:
      _toSend = (MSG*)malloc(6);
      answer = _toSend;
      //answer = (MSG*)malloc(6);

      answer->MsgType = MSG_TYPE_SETTING_AFFECTATION;
      answer->SettingAffectation.DataType = msg->SettingRequest.DataType;
      switch (msg->SettingRequest.DataType) {
      case DATA_TYPE_X_OFFSET: answer->SettingAffectation.Value = _settings->XOffset; break;
      case DATA_TYPE_Y_OFFSET: answer->SettingAffectation.Value = _settings->XScale; break;
      case DATA_TYPE_X_SCALE: answer->SettingAffectation.Value = _settings->YOffset; break;
      case DATA_TYPE_Y_SCALE: answer->SettingAffectation.Value = _settings->YScale; break;
      }
      //write((byte*)answer, 6);
      //free(answer);
      break;
   case MSG_TYPE_STITCH_AFFECTATION:
      break;
   }
}

void SerialHelper::send() {
   if (_toSend == 0) return;

   write((byte*)_toSend, 6);
   free(_toSend);
   _toSend = 0;
}

void SerialHelper::write(byte* data, int size) {
   byte chk = 0;
   int sendSize = size + 5;

   for (int i = 0; i < size; i++)
      chk += data[i];

   _serial->write(STX);
   _serial->write(sendSize % 256);
   _serial->write(sendSize / 256);
   _serial->write(chk);

   for (int i = 0; i < size; i++)
      _serial->write(data[i]);

   _serial->write(ETX);
}