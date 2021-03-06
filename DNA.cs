﻿using System;

namespace PopulationGenerator
{
	public enum Gender
	{
		Male,
		Female
	}

	public enum HairColor
	{
		Black,
		Brown,
		Blonde,
		Ginger
	};

	public enum EyeColor
	{
		Brown,
		Blue,
		Hazel,
		Green
	};

	public enum SkinColor
	{
		White,
		Black
	};

	public enum Height
	{
		CM_160,
		CM_161,
		CM_162,
		CM_163,
		CM_164,
		CM_165,
		CM_166,
		CM_167,
		CM_168,
		CM_169,
		CM_170,
		CM_171,
		CM_172,
		CM_173,
		CM_174,
		CM_175
	};

	public sealed class DNA
	{
		private static uint uniqueID = 0;
		public uint UniqueID { get; private set; } = uniqueID++;

		public Gender Gender { get; private set; }
		public HairColor HairColor { get; private set; }
		public EyeColor EyeColor { get; private set; }
		public SkinColor SkinColor { get; private set; }
		public Height Height { get; private set; }
		public BloodType BloodType { get; private set; }

		public static DNA Generate()
		{
			return new DNA(Utils.Pick<Gender>(), Utils.Pick<HairColor>(), Utils.Pick<EyeColor>(), Utils.Pick<SkinColor>(), Utils.Pick<Height>(), new BloodType(Utils.Pick<BloodGroup>(), Utils.Pick<BloodRHFactor>()));
		}

		public DNA(Gender gender, HairColor hairColor, EyeColor eyeColor, SkinColor skinColor, Height height, BloodType bloodType)
		{
			Gender = gender;
			HairColor = hairColor;
			EyeColor = eyeColor;
			SkinColor = skinColor;
			Height = height;
			BloodType = bloodType;
		}

		public DNA(DNA mother, DNA father)
		{
			Gender = Utils.Pick<Gender>();
			HairColor = Utils.Pick(mother.HairColor, father.HairColor);
			EyeColor = Utils.Pick(mother.EyeColor, father.EyeColor);
			SkinColor = Utils.Pick(mother.SkinColor, father.SkinColor);
			Height = (Height)Utils.Rnd.Next(Math.Min((int)mother.Height, (int)father.Height), Math.Max((int)mother.Height, (int)father.Height) + 1);
			BloodType = BloodType.Derive(mother.BloodType, father.BloodType);
		}
	}
}
