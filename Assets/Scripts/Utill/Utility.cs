using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;



namespace Assets.Scripts
{
    public static class Utility 
    {
        
        // 등비 수열 구간 합 
        public static BigInteger GeometricSumInRange(BigInteger a,double r, int start, int end) 
        {
            if (start >= end)
                return 0;

            var oToE = GeometricSum(a, r, end);
            var oToS = GeometricSum(a, r, start);

            return oToE - oToS;
        }
        // 등비 수열 합 
        public static BigInteger GeometricSum(BigInteger a,double r, int n) 
        {

            if (r == 1)
            {
                return a * n; 
            }
            else
            {  
                return BigIntMult(a,(1 - Math.Pow(r,n))/(1-r));
            }
        }
        // 등비 수열 일반항  
        public static BigInteger GeoProgression(BigInteger a,double r, int n) 
        {
            if(r == 1)
                return a * n;

            Debug.Log(Math.Pow(r, n - 1));

            return BigIntMult(a, Math.Pow(r, n - 1));
        }
    

        public static BigInteger BigIntMult(BigInteger a,double r,int scale =  1000000000)
        {     
           
           
            BigInteger result = a * (BigInteger)(r * scale)/ scale;    
            
            return result;
        }

        // public static decimal DecimalPow(decimal r,int n)
        // {
        //     decimal result = 1m;
        //     decimal baseValue = r;

        //     while(n > 0)
        //     {
        //         if(n % 2 == 1)
        //         {
        //             result *= baseValue;

        //         }

        //         baseValue *= baseValue;
        //         n /= 2;
        //     }


        //     return result;

        // }


        public static string FormatNumberKoreanUnit(BigInteger value)
        {
            var result = "";

            int cnt = 0;
            BigInteger a = 10000; // 만
            BigInteger b = 1_0000_0000; // 억
            BigInteger c = 1_0000_0000_0000; // 조
            BigInteger d = 1_0000_0000_0000_0000; // 경
            BigInteger e = BigInteger.Parse("100000000000000000000"); // 해
            BigInteger f = BigInteger.Parse("1000000000000000000000000"); // 자
            BigInteger g = BigInteger.Parse("10000000000000000000000000000"); // 양
            BigInteger h = BigInteger.Parse("100000000000000000000000000000000"); // 구
            BigInteger i = BigInteger.Parse("1000000000000000000000000000000000000"); // 간

            if (value >= i && cnt < 2)
            {
                cnt++;
                result += $"{value / i}간 ";
                value %= i;
            }
            if (value >= h && cnt < 2)
            {
                cnt++;
                result += $"{value / h}구 ";
                value %= h;
            }
            if (value >= g && cnt < 2)
            {
                cnt++;
                result += $"{value / g}양 ";
                value %= g;
            }
            if (value >= f && cnt < 2)
            {
                cnt++;
                result += $"{value / f}자 ";
                value %= f;
            }
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

        public static void LoadSprite(string address, Action<Sprite> onLoaded)
        {
            var handle = Addressables.LoadAssetAsync<Sprite>(address);
            handle.Completed += (sprite) =>
            {
                if (sprite.Status == AsyncOperationStatus.Succeeded)
                {
                    onLoaded?.Invoke(sprite.Result);
                }
                else
                {
                    Debug.LogWarning($"Failed to load sprite at address: {address}");
                    onLoaded?.Invoke(null);
                }
            };
        }


    }


}
