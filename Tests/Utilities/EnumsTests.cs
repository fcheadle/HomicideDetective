using HomicideDetective.Old.Creatures.Components;
using HomicideDetective.Old.Utilities;
using NUnit.Framework;

namespace Tests.Utilities
{
    class EnumsTests
    {

        [Test]
        [Category("NonGraphical")]
        public void EnumValueFromIndexTest()
        {
            BloodType first = 0;
            int amountOfTypes = EnumUtils.EnumLength<BloodType>();
            BloodType last = (BloodType)(amountOfTypes - 1);
            Assert.AreNotEqual(first, last);
            last -= amountOfTypes;
            last++;
            Assert.AreEqual(first, last);
        }
        [Test]
        [Category("NonGraphical")]
        public void EnumLengthTest()
        {
            int answer = EnumUtils.EnumLength<BloodType>();
            Assert.AreEqual(4, answer);
        }
        [Test]
        [Category("NonGraphical")]
        public void RandomEnumValueTest()
        {
            BloodType type = EnumUtils.RandomEnumValue<BloodType>();
            bool makesnew = false;
            bool makesA = false;
            bool makesAb = false;
            bool makesO = false;
            bool makesB = false;
            for (int i = 0; i < 50; i++)
            {
                if (EnumUtils.RandomEnumValue<BloodType>() != type)
                    makesnew = true;
                if (type == BloodType.A)
                    makesA = true;
                if (type == BloodType.B)
                    makesB = true;
                if (type == BloodType.AB)
                    makesAb = true;
                if (type == BloodType.O)
                    makesO = true;

                type = EnumUtils.RandomEnumValue<BloodType>();
            }

            Assert.IsTrue(makesnew);
            Assert.IsTrue(makesA);
            Assert.IsTrue(makesB);
            Assert.IsTrue(makesAb);
            Assert.IsTrue(makesO);

        }

    }
}
