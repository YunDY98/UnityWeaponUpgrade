using System;
using System.Numerics;



namespace Assets.Scripts
{
    public static class Utility 
    {


    //    public static BigInteger BigIntMult(BigInteger big,float mult,int scale)
    //    {    
    //         if (big < scale)
    //         {
    //             throw new ArgumentException(
    //                 $"[BigIntMult] big 값({big})이 scale({scale})보다 작습니다. 정수 연산으로 인해 결과가 손실될 수 있습니다.");

    //         }
            
    //         int decimalDigits = GetDecimalDigits(mult);

    //         //scale이 표현 가능한 자릿수보다 작으면 예외 발생
    //         int scaleDigits = (int)Math.Log10(scale);
    //         if (decimalDigits > scaleDigits)
    //         {
    //             throw new ArgumentException(
    //                 $"mult 값({mult})의 소수점 자릿수({decimalDigits})가 scale({scale})로 표현 가능한 자릿수({scaleDigits})를 초과합니다.");
                
    //         }



    //         BigInteger cost = BigInteger.Parse("1000");
    //         BigInteger costMultiply = (BigInteger)(mult*scale);
    //         BigInteger result = cost * costMultiply / scale;



    //         return result;
    //    }

        public static string FormatNumberKoreanUnit(BigInteger value)
        {
            var result = "";

            int cnt = 0;
            BigInteger a = 10_000;// 만
            BigInteger b = 1_0000_0000; // 억
            BigInteger c = 1_0000_0000_0000; // 조
            BigInteger d = 1_0000_0000_0000_0000; // 경
            BigInteger e =  BigInteger.Parse("100000000000000000000"); // 해


            if (value >= e && cnt < 2)
            {
                cnt++;
                result += $"{value / e}해 ";
                value %= e;


            }
            if (value >= d && cnt < 2)
            {
                cnt++;
                result += $"{value / d}경 ";
                value %= d;


            }
            if (value >= c && cnt < 2)
            {
                cnt++;
                result += $"{value / c}조 ";
                value %= c;
            }
            if (value >= b && cnt < 2)
            {
                cnt++;
                result += $"{value / b}억 ";
                value %= b;

            }
            if (value >= a && cnt < 2)
            {
                cnt++;
                result += $"{value / a}만 ";
                value %= a;
            }
            if (value > 0 && cnt < 2)
            {
                cnt++;
                result += $"{value}";
            }

            return string.IsNullOrEmpty(result) ? "0" : result.Trim();
        }



    }


}
