using GoRogue.GameFramework;
using GoRogue.GameFramework.Components;
using SadConsole.Components;
using SadConsole.Components.GoRogue;
namespace Engine.Components
{
    class HealthComponent : ComponentBase
    {
        public int SystoleBloodPressure { get => _systoleBloodPressure; set => _systoleBloodPressure = value; }
        public int DiastoleBloodPressure { get => _diastoleBloodPressure; set => _diastoleBloodPressure = value; }
        public int Pulse { get => _pulse; set => _pulse = value; }
        public int BloodVolume { get => _bloodVolume; set => _bloodVolume = value; }
        public double BodyTemperature { get => _bodyTemperature; set => _bodyTemperature = value; }

        private int _systoleBloodPressure;
        private int _diastoleBloodPressure;
        private int _pulse;
        private int _bloodVolume;
        private double _bodyTemperature;
        private int _timeSinceLastHeartbeat = 0;

        public HealthComponent(int systoleBloodPressure = 120, int diastoleBloodPressure = 80, int pulse = 85, int bloodVolume = 5000, double bodyTemperature = 96.7) 
            : base(isUpdate: false, isKeyboard: false, isDraw: false, isMouse: false)
        {
            SystoleBloodPressure = systoleBloodPressure;
            DiastoleBloodPressure = diastoleBloodPressure;
            Pulse = pulse;
            BloodVolume = bloodVolume;
            BodyTemperature = bodyTemperature;
        }

        public override void ProcessGameFrame()
        {
            //health status check?
            _timeSinceLastHeartbeat++;
            if(1 / _pulse < _timeSinceLastHeartbeat)
            {
                _timeSinceLastHeartbeat = 0;
            }
        }
        public override string ToString()
        {
            //this person is the picture of health...
            return base.ToString();
        }
    }
}
