namespace HomicideDetective.People
{
    public class BodyLanguage
    {
        public string CurrentStance { get; private set; } = "";
        public string CurrentPosture { get; private set; } = "";
        public string CurrentArmPosition { get; private set; } = "";
        public string CurrentEyeMovements { get; private set; } = "";
        public string CurrentLipPosition { get; private set; } = "";
        public string CurrentEyebrows { get; private set; } = "";

        public void NextBodyLanguage(bool shifty)
        {
            GetNewStance(shifty);
            GetNewPosture(shifty);
            GetNewArmPosition(shifty);
            GetNewEyeMovements(shifty);
            GetNewLipPosition(shifty);
            GetNewEyebrowPosition(shifty);
        }

        private void GetNewArmPosition(bool shifty)
        {
        }

        private void GetNewEyeMovements(bool shifty)
        {
        }

        private void GetNewLipPosition(bool shifty)
        {
        }

        private void GetNewEyebrowPosition(bool shifty)
        {
        }

        public void GetNewStance(bool shifty)
        {
        }
        public void GetNewPosture(bool shifty)
        {
        }
    }
}