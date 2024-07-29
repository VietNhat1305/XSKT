﻿
using Newtonsoft.Json;

namespace XoSoKienThiet.FromBodyModels
{
    public class IdFromBodyModel
    {
        [JsonProperty(PropertyName = "_id")]
        public string? Id { get; set; }
    }
    public class IdFromBodyDaoTaoModel : IdFromBodyModel
    {
        public string Code { get; set; }
    }
    
}