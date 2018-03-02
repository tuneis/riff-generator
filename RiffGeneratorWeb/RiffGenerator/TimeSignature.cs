namespace RiffGeneratorWeb.RiffGenerator
{
    public class TimeSignature
    {
        public byte Numerator { get; set; }
        public byte Denominator { get; set; }

        public TimeSignature(byte numerator, int denominator)
        {
            // numerator/denominator Ex. 4/4, 3/4
            // check the denominator
            byte _denominator;

            // default to 4/4 time
            switch (denominator)
            {
                case 1:
                    _denominator = 0;
                    break;
                case 2:
                    _denominator = 1;
                    break;
                case 4:
                    _denominator = 2;
                    break;
                case 8:
                    _denominator = 3;
                    break;
                case 16:
                    _denominator = 4;
                    break;
                case 32:
                    _denominator = 5;
                    break;
                default:
                    _denominator = 2;
                    break;
            }

            this.Numerator = numerator;
            this.Denominator = _denominator;
        }
    }
}
