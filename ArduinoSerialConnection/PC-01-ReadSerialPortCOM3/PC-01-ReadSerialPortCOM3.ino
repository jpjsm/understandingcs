int iteration = 0;
char out = 'A';

void setup() {
  // put your setup code here, to run once:
  Serial.begin(57600);
}

void loop() {
  // put your main code here, to run repeatedly:
  char out = 'A' + (char)(iteration++ % 26);
  Serial.write(out);
}
