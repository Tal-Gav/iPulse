#define USE_ARDUINO_INTERRUPTS false
#include <PulseSensorPlayground.h>

const int OUTPUT_TYPE = SERIAL_PLOTTER;

const int PULSE_INPUT = 36;
const int PULSE_BLINK = 2;    // Pin 13 is the on-board LED
const int PULSE_FADE = 5;
const int THRESHOLD = 2000;   // Adjust this number to avoid noise when idle

byte samplesUntilReport;
const byte SAMPLES_PER_SERIAL_SAMPLE = 10;

PulseSensorPlayground pulseSensor;

#include <WiFi.h>
#include <WiFiClient.h>
#include <WiFiAP.h>

// Set these to your desired credentials.
const char *ssid = "";
const char *password = "";
int isClient = 0;
int size;
int bpm = 0;
WiFiServer server(80);
WiFiClient client;

/*
* The function is responsible to start an access point 
* and setting up the BPM sensor.
* Input: None
* Output: None
*/

void setup() {
  
  Serial.begin(115200);
  Serial.println();
  Serial.println("Configuring access point...");

  WiFi.softAP(ssid, password);
  IPAddress myIP = WiFi.softAPIP();
  Serial.print("AP IP address: ");
  Serial.println(myIP);
  server.begin();
  Serial.println("Server started");
  
  // Configure the PulseSensor manager.
  pulseSensor.analogInput(PULSE_INPUT);
  pulseSensor.blinkOnPulse(PULSE_BLINK);
  pulseSensor.fadeOnPulse(PULSE_FADE);

  pulseSensor.setSerial(Serial);
  pulseSensor.setOutputType(OUTPUT_TYPE);
  pulseSensor.setThreshold(THRESHOLD);

  // Skip the first SAMPLES_PER_SERIAL_SAMPLE in the loop().
  samplesUntilReport = SAMPLES_PER_SERIAL_SAMPLE;

  // Now that everything is ready, start reading the PulseSensor signal.
  if (!pulseSensor.begin()) {
    for(;;) {
      // Flash the led to show things didn't work.
      digitalWrite(PULSE_BLINK, LOW);
      delay(50);
      digitalWrite(PULSE_BLINK, HIGH);
      delay(50);
    }
  }
}

/*
* The function is responsible to check if there
* is a connection to a client and then send
* BPM repeatedly depends on the client's requests. 
* Input: None
* Output: BPM
*/
 
void loop() {

  if(isClient== 0)
  {
      client = server.available();   // listen for incoming clients
    
  }
  if (client) // if you get a client
  {                 
    if(isClient == 0)      
        Serial.print("Connection made successfully :)");
    isClient = 1;  

    if (pulseSensor.sawNewSample()) 
    {
      if (--samplesUntilReport == (byte) 0) 
      {
           if (client.available() > 0) {
              Serial.print("incoming :");
              int size;
              while ((size = client.available()) > 0) {
                uint8_t* msg = (uint8_t*)malloc(size);
                size = client.read(msg, size);
                Serial.write(msg, size);
                free(msg);
                client.print(bpm);
                Serial.println(bpm);
              }
            
            }
    
        samplesUntilReport = SAMPLES_PER_SERIAL_SAMPLE;
        bpm = pulseSensor.getBeatsPerMinute();
        if (pulseSensor.sawStartOfBeat()) 
        {
          pulseSensor.outputBeat();
        }
      }
    }
  }
}
