using HealthFitnessAPI.Constants.Enums;
namespace HealthFitnessAPI.Helpers
{
    public static class EnumHelper
    {
        public static string GetGenderDisplayValue(Gender gender)
        {
            return gender switch
            {
                Gender.Man => "Férfi",
                Gender.Woman => "Nő",
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, message: null)
            };
        }

        public static Gender GetGenderEnumValue(string gender)
        {
            return gender switch
            {
                "Férfi" => Gender.Man,
                "Nő" => Gender.Woman,
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, message: null)
            };
        }
    }
}
