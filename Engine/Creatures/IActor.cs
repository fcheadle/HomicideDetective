namespace Engine.Creatures
{
    public interface IActor
    {
        string Name { get; set; }

        //Health Stats
        int SystoleBloodPressure { get; set; } //90-120
        int DiastoleBloodPressure { get; set; } //80-90
        int Pulse { get; set; } //60-100 BPM
        int BloodVolume { get; set; }
        double BodyTemperature { get; set; }
        int Mass { get; set; }
        int Volume { get; set; }
        public void Act();
    }
}
