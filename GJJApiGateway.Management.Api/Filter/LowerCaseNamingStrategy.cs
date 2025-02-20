using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace GJJApiGateway.Management.Api.Filter
{
    /// <summary>
    /// 自定义一个派生自NamingStrategy的策略类，用于解析属性名称
    /// </summary>
    public class LowerCaseNamingStrategy : NamingStrategy
    {
        /// <summary>
        /// 方法用于将属性名称解析为小写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string ResolvePropertyName(string name)
        {
            try
            {
                // 将传入的名称转化为小写并返回
                return name.ToLower();
            }
            catch
            {
                // 如果转化失败，则直接返回原名称
                return name;
            }
        }
    }

    /// <summary>
    /// 自定义一个派生自 DateTimeConverterBase 的转换器类
    /// </summary>
    public class FixedDateTimeConverter : DateTimeConverterBase
    {
        // 定义要转换的时间格式
        private const string Format = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 禁止进行读取操作
        /// </summary>
        public override bool CanRead => false;
        /// <summary>
        /// 如果试图进行读取操作，抛出未实现的异常
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将对象序列化为JSON时进行的操作
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // 如果传入的对象是DateTime类型，将其转换为特定格式的字符串，并写入JSON
            if (value is DateTime dateTime)
            {
                writer.WriteValue(dateTime.ToString(Format));
            }
            // 否则，抛出未实现的异常
            //throw new NotImplementedException();
        }
    }
}
