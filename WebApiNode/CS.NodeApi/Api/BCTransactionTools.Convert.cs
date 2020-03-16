using System;
using System.Globalization;
using NodeApi;

namespace CS.NodeApi.Api
{
    public sealed class AmountEx
    {
        private Int32 integral;
        private UInt64 fraction;

        public static AmountEx from_decimal(Decimal val)
        {
            AmountEx amount = new AmountEx();

            amount.integral = (Int32)val;
            if (val < 0.0M)
            {
                amount.integral -= 1;
            }

            decimal frac = val - amount.integral;

            frac *= FACTOR;

            //amount.fraction = (UInt64)(frac + 0.5) * multiplier;
            amount.fraction = (UInt64)(frac * MULTIPLIER + 0.5M);
            if (amount.fraction >= AMOUNT_MAX_FRACTION)
            {
                amount.fraction -= AMOUNT_MAX_FRACTION;
                amount.integral += 1;
            }

            return amount;
        }

        public static AmountEx from_double(double val)
        {
            //AmountEx amount = new AmountEx();

            //if (val < (double)Int32.MinValue || val > (double)(Int32.MaxValue))
            //{
            //    throw new OverflowException("Amount::Amount(double) overflow)");
            //}

            //amount.integral = (Int32)val;
            //if (val < 0.0)
            //{
            //    amount.integral -= 1;
            //}

            //double frac = val - (double)amount.integral;

            //frac *= FACTOR;

            ////amount.fraction = (UInt64)(frac + 0.5) * multiplier;
            //amount.fraction = (UInt64)(frac * MULTIPLIER + 0.5);
            //if (amount.fraction >= AMOUNT_MAX_FRACTION)
            //{
            //    amount.fraction -= AMOUNT_MAX_FRACTION;
            //    amount.integral += 1;
            //}

            //return amount;

            throw new NotImplementedException();
        }

        public static AmountEx from_parts(Int32 Integral, UInt64 Fraction)
        {
            return new AmountEx
            {
                integral = Integral,
                fraction = Fraction
            };
        }

        static UInt64 gen_pow(UInt64 b, UInt64 exp)
        {
            return exp == 0 ? 1 : b * gen_pow(b, exp - 1);
        }

        const UInt64 AMOUNT_MAX_FRACTION = 1_000_000_000_000_000_000UL;
        const decimal FACTOR = 1e15M;
        const decimal MULTIPLIER = 1000.0M;

        public Int32 Integral
        {
            get
            {
                return integral;
            }
        }

        public UInt64 Fraction
        {
            get
            {
                return fraction;
            }
        }

        public static string FormatAmount(AmountEx value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            if (value.Fraction == 0)
            {
                return $"{value.Integral}.0";
            }
            string sign = string.Empty;
            if (value.Integral < 0)
            {
                value.fraction = AMOUNT_MAX_FRACTION - value.Fraction;
                value.integral += 1;
                sign = "-";
            }
            var frac = value.Fraction.ToString();
            frac = frac.PadLeft(18, '0');

            return sign + $"{Math.Abs(value.Integral)}.{frac.TrimEnd('0')}";
        }
    }

    public partial class BCTransactionTools
    {


        public static TransactionId GetTransactionIdByStr(string id)
        {
            var list = id.Split('.');
            var result = new TransactionId
            {
                Index = int.Parse(list[1]) - 1,
                PoolSeq = long.Parse(list[0])
            };
            return result;
        }

        public static string ConvertFeeToStr(short c, bool roundFee = false)
        {
            var sign = c >> 15;
            var m = c & 0x3FF;
            var f = (c >> 10) & 0x1F;
            const double v1024 = 1.0 / 1024;
            var num = (sign != 0u ? -1.0 : 1.0) * m * v1024 * Math.Pow(10.0, f - 18);
            if (roundFee) num = Math.Round(num, 5);
            return num.ToString(CultureInfo.InvariantCulture);
        }


        public static AmountCommission EncodeFeeFromDouble(double f)
        {
            f = Math.Abs(f);
            double exp = f == 0 ? 0 : Math.Log10(f);
            int e = Convert.ToInt32(exp >= 0 ? exp + 0.5 : exp - 0.5);
            f /= Math.Pow(10, e);
            if (f >= 1)
            {
                f *= 0.1;
                ++e;
            }
            return new AmountCommission { Commission = (short)((e + 18) << 10 | ((int)(f * 1024) == 1024 ? 1023 : (int)(f * 1024))) };
  
        
        }
        
        public static Amount GetAmountByDouble_C(decimal val)
        {
            AmountEx amountEx = AmountEx.from_decimal(val);
            return new Amount(amountEx.Integral, (long)amountEx.Fraction);
        }

        static UInt64 gen_pow(UInt64 b, UInt64 exp)
        {
            return exp == 0 ? 1 : b * gen_pow(b, exp - 1);
        }
    }


    //class Amount
    //{
    //    public Int32 integral;
    //    public UInt64 fraction;



    //    const UInt64 AMOUNT_MAX_FRACTION = 1000000000000000000UL;
    //    const UInt64 DIGITS = 15UL; // C++ std::numeric_limits<double>::digits10

    //}
}
