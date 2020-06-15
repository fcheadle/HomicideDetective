using Engine.Creatures.Components;
using Engine.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Utilities
{
    class EnumsTests
    {

        [Test]
        public void EnumValueFromIndexTest()
        {
            BloodType first = (BloodType)0;
            int amountOfTypes = EnumUtils.EnumLength<BloodType>();
            BloodType last = (BloodType)(amountOfTypes - 1);
            Assert.AreNotEqual(first, last);
            last -= amountOfTypes;
            last++;
            Assert.AreEqual(first, last);
        }
        [Test]
        public void EnumLengthTest()
        {
            int answer = EnumUtils.EnumLength<BloodType>();
            Assert.AreEqual(4, answer);
        }
        [Test]
        public void RandomEnumValueTest()
        {
            BloodType type = EnumUtils.RandomEnumValue<BloodType>();
            bool makesnew = false;
            bool makesA = false;
            bool makesAB = false;
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
                    makesAB = true;
                if (type == BloodType.O)
                    makesO = true;

                type = EnumUtils.RandomEnumValue<BloodType>();
            }

            Assert.IsTrue(makesnew);
            Assert.IsTrue(makesA);
            Assert.IsTrue(makesB);
            Assert.IsTrue(makesAB);
            Assert.IsTrue(makesO);

        }

    }
}
