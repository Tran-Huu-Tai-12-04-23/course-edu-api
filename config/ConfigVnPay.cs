namespace course_edu_api.config;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

public class ConfigVnPay
    {
        public static string vnp_Version = "2.1.0";
        public static string vnp_Command = "querydr";
        public static string vnp_PayUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        public static string vnp_ReturnUrl = "http://localhost:5173/course/register/payment-notification";
        public static string vnp_TmnCode = "96SVMJBD";
        public static string secretKey = "BNLXRZCTVEQVBZSCXABYBJPOLLEGFXLB";
        public static string vnp_ApiUrl = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";

        public static string Md5(string message)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(message);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static string Sha256(string message)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(message);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static string HashAllFields(Dictionary<string, string> fields)
        {
            var fieldNames = fields.Keys.ToList();
            fieldNames.Sort();
            StringBuilder sb = new StringBuilder();
            foreach (string fieldName in fieldNames)
            {
                string fieldValue = fields[fieldName];
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    sb.Append(fieldName);
                    sb.Append("=");
                    sb.Append(fieldValue);
                }
                if (fieldNames.IndexOf(fieldName) < fieldNames.Count - 1)
                {
                    sb.Append("&");
                }
            }
            return HmacSHA512(secretKey, sb.ToString());
        }

        public static string HmacSHA512(string key, string data)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static string GetRandomNumber(int len)
        {
            Random rnd = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, len).Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        public static string GetIpAddress(HttpRequest request)
        {
            string ipAdress;
            try
            {
                ipAdress = request.Headers["X-FORWARDED-FOR"].FirstOrDefault();
                if (string.IsNullOrEmpty(ipAdress))
                {
                    ipAdress = request.HttpContext.Connection.RemoteIpAddress.ToString();
                }
            }
            catch (Exception e)
            {
                ipAdress = "Invalid IP:" + e.Message;
            }
            return ipAdress;
        }
    }
