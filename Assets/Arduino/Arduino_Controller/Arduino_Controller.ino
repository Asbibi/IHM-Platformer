#include <CapacitiveSensor.h>

int gravityReadPin = A0;
int deathPin = 2;         // Digital
int groundedPin = 7;      // Digital
//int gravityPin = 3;       // Analog
int slowmoPinSend = 5;    // Analog
int slowmoPinReceive = 11; // Analog
CapacitiveSensor touch(slowmoPinSend,slowmoPinReceive);

bool deathFlag = false;


void setup() {    
  pinMode(gravityReadPin,INPUT);
  pinMode(deathPin,OUTPUT);
  pinMode(groundedPin,OUTPUT);
  //pinMode(gravityPin,INPUT);
  
  // 115200 is a common baudrate : fast without being overwhelming
  Serial.begin(9600);//115200);{   
}

// Processes button input
void loop() {
  if (deathFlag){
    ledClignote(deathPin, 10, 50);
    deathFlag = false;
  }
  
  delay(100);
  setGravity();
  
  delay(100);
  setSlowMotion();
}

void setGravity(){
  int value = analogRead(gravityReadPin);
  //Serial.println(value);
  
  if (value > 860)
    value = 860;
  else if (value < 0)
    value = 0;
  
  Serial.print("G_");
  Serial.println(value);
}

void setSlowMotion(){
  long value = touch.capacitiveSensor(30);
  //Serial.println(value);
 
  if (value > 1500) {
    Serial.println("SL");
  }
  else {
    Serial.println("NSL");
  }
}


void serialEvent()
{
  String message = Serial.readStringUntil('\n');
  if (message == "D\r") {
    deathFlag = true;
    //digitalWrite(groundedPin,HIGH);
  } else if (message == "GRD_ON\r") {
    digitalWrite(groundedPin,HIGH);
  } else if (message == "GRD_OFF\r") {
    digitalWrite(groundedPin,LOW);
  }
}

void ledClignote(int pin, int times, int delayCligno){
  for(int i=0; i<times; i++){    
    digitalWrite(pin,HIGH);
    delay(delayCligno);   
    digitalWrite(pin,LOW);
    delay(delayCligno);
  }  
}

/*
// The flag signals to the rest of the program an interrupt occured
static bool button_flag = false;
// Remember the state the river in the Unity program is in
static bool river_state = false;

// Interrupt handler, sets the flag for later processing
void buttonPress() {
  button_flag = true;
}

void setup() {
  int buttonPin = 2;
  
  pinMode(LED_BUILTIN, OUTPUT);
  // Internal pullup, no external resistor necessary
  pinMode(buttonPin,INPUT_PULLUP);
  // 115200 is a common baudrate : fast without being overwhelming
  Serial.begin(115200);

  // As the button is in pullup, detect a connection to ground
  attachInterrupt(digitalPinToInterrupt(buttonPin),buttonPress,FALLING);
}

// Processes button input
void loop() {
  // Slows reaction down a bit
  // but prevents _most_ button press misdetections
  delay(200);
  
  if (button_flag) {
    if (river_state) {
      Serial.println("dry");
    } else {
      Serial.println("wet");
    }
    river_state = !river_state;
    button_flag = false;
  }
}

// Handles incoming messages
// Called by Arduino if any serial data has been received
void serialEvent()
{
  String message = Serial.readStringUntil('\n');
  if (message == "LED ON\r") {
    digitalWrite(13,HIGH);
  } else if (message == "LED OFF\r") {
    digitalWrite(13,LOW);
  }
}
*/
