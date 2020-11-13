#ifndef SERIAL_HELPER
#define SERIAL_HELPER

#include <NeoSWSerial.h>
#include "Arduino.h"
#include "Settings.h"

#define STX 0x02
#define ETX 0x03

typedef enum { // Size : 2
   MSG_TYPE_SETTING_AFFECTATION = 1,
   MSG_TYPE_SETTING_REQUEST = 2,
   MSG_TYPE_STITCH_AFFECTATION = 3,
   MSG_TYPE_ERROR = 99,
}MSG_TYPE;

typedef enum { // Size : 2
   DATA_TYPE_X_OFFSET = 1,
   DATA_TYPE_Y_OFFSET = 2,
   DATA_TYPE_X_SCALE = 3,
   DATA_TYPE_Y_SCALE = 4,
}DATA_TYPE;

typedef struct {
   MSG_TYPE MsgType;
   union {
      struct {
         DATA_TYPE DataType;
         float Value;
      }SettingAffectation;
      struct {
         DATA_TYPE DataType;
      }SettingRequest;
      struct {

      }StitchAffectation;
   };
}MSG;



class SerialHelper {
public:
   SerialHelper(Settings* settings, int rxPin, int txPin, int baud);
   void init();
   void handleRxChar(uint8_t c);
   void send();
private:
   int _rxPin;
   int _txPin;
   int _baud;

   MSG* _toSend;


   byte* _data;
   int _dataIdx;
   int _dataSize;
   int _chkValue;
   int _chkCalc;

   NeoSWSerial* _serial;
   Settings* _settings;
   void manageMsg(byte* data, int size);
   void write(byte* data, int size);
};

#endif // !SERIAL_HELPER
