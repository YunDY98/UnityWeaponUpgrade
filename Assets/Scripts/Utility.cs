using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;



namespace Assets.Scripts
{
    public static class Utility 
    {
        public static BigInteger BigIntMult(BigInteger cand,float rate,int scale = 1000)
        {    
        

            BigInteger  mult = (BigInteger)(rate * scale);
           
            BigInteger result = cand * mult / scale;


            
            return result;
        }

        public static BigInteger GeoProgression(BigInteger startValue, float rate, int level)
        {
            if(rate == 1)
                return startValue * (int)rate * level;
           
            return BigIntMult(startValue,Mathf.Pow(rate, level - 1));
        }


        public static string FormatNumberKoreanUnit(BigInteger value)
        {
            var result = "";

            int cnt = 0;
            BigInteger a = 10000;// 만
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
