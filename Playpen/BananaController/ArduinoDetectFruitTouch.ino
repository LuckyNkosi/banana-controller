const int threshold = 875;
void setup() 
{
  Serial.begin(9600);
  Keyboard.begin();
}

void loop()
{
  int val = 0;
  if (analogRead(A0) < threshold)
  {
    Serial.write(1);
    Serial.flush();
    delay(80);
  }
  
  if (analogRead(A5) < threshold)
  {
    Serial.write(2);
    Serial.flush();
    delay(80);
  }
}