﻿using System.ComponentModel;

namespace Selenium.Utilities.Tags
{
    public enum TagAttributes
    {
        [Description("id")]
        Id,

        [Description("name")]
        Name,

        [Description("class")]
        Class,

        [Description("value")]
        Value,

        [Description("onclick")]
        OnClick,

        [Description("src")]
        Src,

        [Description("title")]
        Title,

        [Description("href")]
        Href,

        [Description("type")]
        Type,
       
        [Description("style")]
        Style,

        [Description("rel")]
        Rel,

        [Description("for")]
        For,

        [Description("data-addressid")]
        DataAddressId
    }
}
