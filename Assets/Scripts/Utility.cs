using System;
using System.Numerics;
using UnityEngine;





namespace Assets.Scripts
{
    public static class Utility 
    {
        //float double로 인한 실제 값이랑 오차가 있음 decimal 사용
        // 등비 수열 구간 합 
        public static BigInteger GeometricSumInRange(BigInteger a, double r, int start, int end) 
        {
            
            
           
          
            var oToE = GeometricSum(a, r, end);
          

            
            var oToS = GeometricSum(a, r, start);

            

            return oToE - oToS;
        }
        // 등비 수열 합 
        public static BigInteger GeometricSum(BigInteger a,double r, int n) 
        {

            if(n <= 0)
                return 0;
            if (r == 1)
            {
                return a * n; 
            }
            else
            {
               
              
                 
                return BigIntDiv(BigIntMult(a,1 - Math.Pow(r, n)),1 - r);  
            }
        }
        // 등비 수열 항 값 
        public static BigInteger GeoProgression(BigInteger a, float r, int n) 
        {
            if(r == 1)
                return a * n;

                
            return BigIntMult(a,Mathf.Pow(r, n - 1));
        }


        public static BigInteger BigIntDiv(BigInteger a, double r, int scale = 1000000000)
        {
            
            BigInteger div = (BigInteger)(r * scale);

          
            BigInteger result = a * scale / div;

            return result;
        }

        public static BigInteger BigIntMult(BigInteger a,double r,int scale =  1000000000)
        {    
        

            BigInteger  mult = (BigInteger)(r * scale);
           
            BigInteger result = a * mult / scale;    
            
            return result;
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
