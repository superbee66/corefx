// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

public static class DateTimeTests
{
    [Fact]
    public static void TestMaxValue()
    {
        VerifyDateTime(DateTime.MaxValue, 9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified);
    }

    [Fact]
    public static void TestMinValue()
    {
        VerifyDateTime(DateTime.MinValue, 1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
    }

    [Fact]
    public static void TestCtor_Long()
    {
        VerifyDateTime(new DateTime(999999999999999999), 3169, 11, 16, 9, 46, 39, 999, DateTimeKind.Unspecified);
    }

    [Fact]
    public static void TestCtor_Long_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>("ticks", () => new DateTime(DateTime.MinValue.Ticks - 1)); // Ticks < DateTime.MinValue.Ticks
        Assert.Throws<ArgumentOutOfRangeException>("ticks", () => new DateTime(DateTime.MaxValue.Ticks + 1)); // Ticks > DateTime.MaxValue.Ticks
    }

    [Fact]
    public static void TestCtor_Long_DateTimeKind()
    {
        VerifyDateTime(new DateTime(999999999999999999, DateTimeKind.Utc), 3169, 11, 16, 9, 46, 39, 999, DateTimeKind.Utc);
    }

    [Fact]
    public static void TestCtor_Long_DateTimeKind_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>("ticks", () => new DateTime(DateTime.MinValue.Ticks - 1, DateTimeKind.Utc)); // Ticks < DateTime.MinValue.Ticks
        Assert.Throws<ArgumentOutOfRangeException>("ticks", () => new DateTime(DateTime.MaxValue.Ticks + 1, DateTimeKind.Utc)); // Ticks > DateTime.MaxValue.Ticks

        Assert.Throws<ArgumentException>("kind", () => new DateTime(0, DateTimeKind.Unspecified - 1)); // Invalid date time kind
        Assert.Throws<ArgumentException>("kind", () => new DateTime(0, DateTimeKind.Local + 1)); // Invalid date time kind
    }

    [Fact]
    public static void TestCtor_Int_Int_Int()
    {
        var dateTime = new DateTime(2012, 6, 11);
        VerifyDateTime(dateTime, 2012, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified);
    }

    [Fact]
    public static void TestCtor_Int_Int_Int_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(0, 1, 1)); // Year < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(10000, 1, 1)); // Year > 9999

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 0, 1)); // Month < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 13, 1)); // Month > 12

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 0)); // Day < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 32)); // Day > days in month
    }

    [Fact]
    public static void TestCtor_Int_Int_Int_Int_Int_Int()
    {
        var dateTime = new DateTime(2012, 12, 31, 13, 50, 10);
        VerifyDateTime(dateTime, 2012, 12, 31, 13, 50, 10, 0, DateTimeKind.Unspecified);
    }

    [Fact]
    public static void TestCtor_Int_Int_Int_Int_Int_Int_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(0, 1, 1, 1, 1, 1)); // Year < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(10000, 1, 1, 1, 1, 1)); // Year > 9999

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 0, 1, 1, 1, 1)); // Month < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 13, 1, 1, 1, 1)); // Month > 12

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 0, 1, 1, 1)); // Day < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 32, 1, 1, 1)); // Day > days in month

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, -1, 1, 1)); // Hour < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 24, 1, 1)); // Hour > 23

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, -1, 1)); // Minute < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 60, 1)); // Minute > 59

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 1, -1)); // Second < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 1, 60)); // Second > 59
    }

    [Fact]
    public static void TestCtor_Int_Int_Int_Int_Int_Int_Int_DateTimeKind()
    {
        var dateTime = new DateTime(1986, 8, 15, 10, 20, 5, DateTimeKind.Local);
        VerifyDateTime(dateTime, 1986, 8, 15, 10, 20, 5, 0, DateTimeKind.Local);
    }

    [Fact]
    public static void TestCtor_Int_Int_Int_Int_Int_Int_DateTimeKind_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(0, 1, 1, 1, 1, 1, DateTimeKind.Utc)); // Year < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(10000, 1, 1, 1, 1, 1, DateTimeKind.Utc)); // Year > 9999

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 0, 1, 1, 1, 1, DateTimeKind.Utc)); // Month < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 13, 1, 1, 1, 1, DateTimeKind.Utc)); // Month > 12

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 0, 1, 1, 1, DateTimeKind.Utc)); // Day < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 32, 1, 1, 1, DateTimeKind.Utc)); // Day > days in month

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, -1, 1, 1, DateTimeKind.Utc)); // Hour < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 24, 1, 1, DateTimeKind.Utc)); // Hour > 23

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, -1, 1, DateTimeKind.Utc)); // Minute < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 60, 1, DateTimeKind.Utc)); // Minute > 59

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 1, -1, DateTimeKind.Utc)); // Second < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 1, 60, DateTimeKind.Utc)); // Second > 59

        Assert.Throws<ArgumentException>("kind", () => new DateTime(1, 1, 1, 1, 1, 1, DateTimeKind.Unspecified - 1)); // Invalid date time kind
        Assert.Throws<ArgumentException>("kind", () => new DateTime(1, 1, 1, 1, 1, 1, DateTimeKind.Local + 1)); // Invalid date time kind
    }

    [Fact]
    public static void TestCtor_Int_Int_Int_Int_Int_Int_Int()
    {
        var dateTime = new DateTime(1973, 10, 6, 14, 30, 0, 500);
        VerifyDateTime(dateTime, 1973, 10, 6, 14, 30, 0, 500, DateTimeKind.Unspecified);
    }

    [Fact]
    public static void TestCtor_Int_Int_Int_Int_Int_Int_Int_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(0, 1, 1, 1, 1, 1, 1)); // Year < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(10000, 1, 1, 1, 1, 1, 1)); // Year > 9999

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 0, 1, 1, 1, 1, 1)); // Month < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 13, 1, 1, 1, 1, 1)); // Month > 12

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 0, 1, 1, 1, 1)); // Day < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 32, 1, 1, 1, 1)); // Day > days in month

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, -1, 1, 1, 1)); // Hour < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 24, 1, 1, 1)); // Hour > 23

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, -1, 1, 1)); // Minute < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 60, 1, 1)); // Minute > 59

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 1, -1, 1)); // Second < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 1, 60, 1)); // Second > 59

        Assert.Throws<ArgumentOutOfRangeException>("millisecond", () => new DateTime(1, 1, 1, 1, 1, 1, -1)); // Milisecond < 0
        Assert.Throws<ArgumentOutOfRangeException>("millisecond", () => new DateTime(1, 1, 1, 1, 1, 1, 1000)); // Millisecond > 999
    }

    [Fact]
    public static void TestCtor_Int_Int_Int_Int_Int_Int_Int_Int_DateTimeKind()
    {
        var dateTime = new DateTime(1986, 8, 15, 10, 20, 5, 600, DateTimeKind.Local);
        VerifyDateTime(dateTime, 1986, 8, 15, 10, 20, 5, 600, DateTimeKind.Local);
    }

    [Fact]
    public static void TestCtor_Int_Int_Int_Int_Int_Int_Int_DateTimeKind_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(0, 1, 1, 1, 1, 1, 1, DateTimeKind.Utc)); // Year < 1
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(10000, 1, 1, 1, 1, 1, 1, DateTimeKind.Utc)); // Year > 9999

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 0, 1, 1, 1, 1, 1, DateTimeKind.Utc)); // Month < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 13, 1, 1, 1, 1, 1, DateTimeKind.Utc)); // Month > 12

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 0, 1, 1, 1, 1, DateTimeKind.Utc)); // Day < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 32, 1, 1, 1, 1, DateTimeKind.Utc)); // Day > days in month

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, -1, 1, 1, 1, DateTimeKind.Utc)); // Hour < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 24, 1, 1, 1, DateTimeKind.Utc)); // Hour > 23

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, -1, 1, 1, DateTimeKind.Utc)); // Minute < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 60, 1, 1, DateTimeKind.Utc)); // Minute > 59

        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 1, -1, 1, DateTimeKind.Utc)); // Second < 0
        Assert.Throws<ArgumentOutOfRangeException>(null, () => new DateTime(1, 1, 1, 1, 1, 60, 1, DateTimeKind.Utc)); // Second > 59

        Assert.Throws<ArgumentOutOfRangeException>("millisecond", () => new DateTime(1, 1, 1, 1, 1, 1, -1, DateTimeKind.Utc)); // Millisecond < 0
        Assert.Throws<ArgumentOutOfRangeException>("millisecond", () => new DateTime(1, 1, 1, 1, 1, 1, 1000, DateTimeKind.Utc)); // Millisecond > 999

        Assert.Throws<ArgumentException>("kind", () => new DateTime(1, 1, 1, 1, 1, 1, 1, DateTimeKind.Unspecified - 1)); // Invalid date time kind
        Assert.Throws<ArgumentException>("kind", () => new DateTime(1, 1, 1, 1, 1, 1, 1, DateTimeKind.Local + 1)); // Invalid date time kind
    }

    [Theory]
    [InlineData(2004, true)]
    [InlineData(2005, false)]
    public static void TestLeapYears(int year, bool expected)
    {
        Assert.Equal(expected, DateTime.IsLeapYear(year));
    }

    [Fact]
    public static void TestLeapYears_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>("year", () => DateTime.IsLeapYear(0));
        Assert.Throws<ArgumentOutOfRangeException>("year", () => DateTime.IsLeapYear(10000));
    }

    [Fact]
    public static void TestAddition()
    {
        var dateTime = new DateTime(1986, 8, 15, 10, 20, 5, 70);
        Assert.Equal(17, dateTime.AddDays(2).Day);
        Assert.Equal(13, dateTime.AddDays(-2).Day);

        Assert.Equal(10, dateTime.AddMonths(2).Month);
        Assert.Equal(6, dateTime.AddMonths(-2).Month);

        Assert.Equal(1996, dateTime.AddYears(10).Year);
        Assert.Equal(1976, dateTime.AddYears(-10).Year);

        Assert.Equal(13, dateTime.AddHours(3).Hour);
        Assert.Equal(7, dateTime.AddHours(-3).Hour);

        Assert.Equal(25, dateTime.AddMinutes(5).Minute);
        Assert.Equal(15, dateTime.AddMinutes(-5).Minute);

        Assert.Equal(35, dateTime.AddSeconds(30).Second);
        Assert.Equal(2, dateTime.AddSeconds(-3).Second);

        Assert.Equal(80, dateTime.AddMilliseconds(10).Millisecond);
        Assert.Equal(60, dateTime.AddMilliseconds(-10).Millisecond);
    }

    [Fact]
    public static void TestDayOfWeek()
    {
        var dateTime = new DateTime(2012, 6, 18);
        Assert.Equal(DayOfWeek.Monday, dateTime.DayOfWeek);
    }

    [Fact]
    public static void TestDayOfYear()
    {
        var dateTime = new DateTime(2012, 6, 18);
        Assert.Equal(170, dateTime.DayOfYear);
    }

    [Fact]
    public static void TestTimeOfDay()
    {
        var dateTime = new DateTime(2012, 6, 18, 10, 5, 1, 0);
        TimeSpan ts = dateTime.TimeOfDay;

        DateTime newDate = dateTime.Subtract(ts);
        Assert.Equal(new DateTime(2012, 6, 18, 0, 0, 0, 0).Ticks, newDate.Ticks);
        Assert.Equal(dateTime.Ticks, newDate.Add(ts).Ticks);
    }

    [Fact]
    public static void TestToday()
    {
        DateTime today = DateTime.Today;
        DateTime now = DateTime.Now;
        VerifyDateTime(today, now.Year, now.Month, now.Day, 0, 0, 0, 0, DateTimeKind.Local);

        today = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc);
        Assert.Equal(DateTimeKind.Utc, today.Kind);
        Assert.False(today.IsDaylightSavingTime());
    }

    [Fact]
    public static void TestConversion()
    {
        DateTime today = DateTime.Today;
        long dateTimeRaw = today.ToBinary();
        Assert.Equal(today, DateTime.FromBinary(dateTimeRaw));

        dateTimeRaw = today.ToUniversalTime().ToBinary();
        Assert.Equal(today.ToUniversalTime(), DateTime.FromBinary(dateTimeRaw));

        dateTimeRaw = today.ToFileTime();
        Assert.Equal(today, DateTime.FromFileTime(dateTimeRaw));

        dateTimeRaw = today.ToFileTimeUtc();
        Assert.Equal(today, DateTime.FromFileTimeUtc(dateTimeRaw).ToLocalTime());
    }

    public static IEnumerable<object[]> Subtract_TimeSpan_TestData()
    {
        var dateTime = new DateTime(2012, 6, 18, 10, 5, 1, 0, DateTimeKind.Utc);

        yield return new object[] { dateTime, new TimeSpan(10, 5, 1), new DateTime(2012, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc) };
        yield return new object[] { dateTime, new TimeSpan(-10, -5, -1), new DateTime(2012, 6, 18, 20, 10, 2, 0, DateTimeKind.Utc) };
    }

    [Theory]
    [MemberData(nameof(Subtract_TimeSpan_TestData))]
    public static void TestSubtract_TimeSpan(DateTime dateTime, TimeSpan timeSpan, DateTime expected)
    {
        Assert.Equal(expected, dateTime - timeSpan);
        Assert.Equal(expected, dateTime.Subtract(timeSpan));
    }

    public static IEnumerable<object[]> Subtract_DateTime_TestData()
    {
        var dateTime1 = new DateTime(1996, 6, 3, 22, 15, 0, DateTimeKind.Utc);
        var dateTime2 = new DateTime(1996, 12, 6, 13, 2, 0, DateTimeKind.Utc);
        var dateTime3 = new DateTime(1996, 10, 12, 8, 42, 0, DateTimeKind.Utc);

        yield return new object[] { dateTime2, dateTime1, new TimeSpan(185, 14, 47, 0) };
        yield return new object[] { dateTime1, dateTime2, new TimeSpan(-185, -14, -47, 0) };
        yield return new object[] { dateTime1, dateTime2, new TimeSpan(-185, -14, -47, 0) };
    }

    [Theory]
    [MemberData(nameof(Subtract_DateTime_TestData))]
    public static void TestSubtract_DateTime(DateTime dateTime1, DateTime dateTime2, TimeSpan expected)
    {
        Assert.Equal(expected, dateTime1 - dateTime2);
        Assert.Equal(expected, dateTime1.Subtract(dateTime2));
    }

    [Fact]
    public static void TestParse_String()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString();

        DateTime result = DateTime.Parse(expectedString);
        Assert.Equal(expectedString, result.ToString());
    }

    [Fact]
    public static void TestParse_String_FormatProvider()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString();

        DateTime result = DateTime.Parse(expectedString, null);
        Assert.Equal(expectedString, result.ToString());
    }

    [Fact]
    public static void TestParse_String_FormatProvider_DateTimeStyles()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString();

        DateTime result = DateTime.Parse(expectedString, null, DateTimeStyles.None);
        Assert.Equal(expectedString, result.ToString());
    }

    [Fact]
    public static void TestParse_Japanese()
    {
        var expected = new DateTime(2012, 12, 21, 10, 8, 6);
        var cultureInfo = new CultureInfo("ja-JP");

        string expectedString = string.Format(cultureInfo, "{0}", expected);
        Assert.Equal(expected, DateTime.Parse(expectedString, cultureInfo));
    }

    [Fact]
    public static void TestTryParse_String()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString("g");

        DateTime result;
        Assert.True(DateTime.TryParse(expectedString, out result));
        Assert.Equal(expectedString, result.ToString("g"));
    }

    [Fact]
    public static void TestTryParse_String_FormatProvider_DateTimeStyles_U()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString("u");

        DateTime result;
        Assert.True(DateTime.TryParse(expectedString, null, DateTimeStyles.AdjustToUniversal, out result));
        Assert.Equal(expectedString, result.ToString("u"));
    }

    [Fact]
    public static void TestTryParse_String_FormatProvider_DateTimeStyles_G()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString("g");

        DateTime result;
        Assert.True(DateTime.TryParse(expectedString, null, DateTimeStyles.AdjustToUniversal, out result));
        Assert.Equal(expectedString, result.ToString("g"));
    }

    [Fact]
    public static void TestTryParse_TimeDesignators()
    {
        DateTime result;
        Assert.True(DateTime.TryParse("4/21 5am", new CultureInfo("en-US"), DateTimeStyles.None, out result));
        Assert.Equal(4, result.Month);
        Assert.Equal(21, result.Day);
        Assert.Equal(5, result.Hour);

        Assert.True(DateTime.TryParse("4/21 5pm", new CultureInfo("en-US"), DateTimeStyles.None, out result));
        Assert.Equal(4, result.Month);
        Assert.Equal(21, result.Day);
        Assert.Equal(17, result.Hour);
    }

    [Fact]
    public static void TestParseExact_String_String_FormatProvider()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString("G");

        DateTime result = DateTime.ParseExact(expectedString, "G", null);
        Assert.Equal(expectedString, result.ToString("G"));
    }

    [Fact]
    public static void TestParseExact_String_String_FormatProvider_DateTimeStyles_U()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString("u");

        DateTime result = DateTime.ParseExact(expectedString, "u", null, DateTimeStyles.None);
        Assert.Equal(expectedString, result.ToString("u"));
    }

    [Fact]
    public static void TestParseExact_String_String_FormatProvider_DateTimeStyles_G()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString("g");

        DateTime result = DateTime.ParseExact(expectedString, "g", null, DateTimeStyles.None);
        Assert.Equal(expectedString, result.ToString("g"));
    }

    [Fact]
    public static void TestParseExact_String_String_FormatProvider_DateTimeStyles_O()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString("o");

        DateTime result = DateTime.ParseExact(expectedString, "o", null, DateTimeStyles.None);
        Assert.Equal(expectedString, result.ToString("o"));
    }

    [Fact]
    public static void TestParseExact_String_String_FormatProvider_DateTimeStyles_CustomFormatProvider()
    {
        var formatter = new MyFormatter();
        string dateBefore = DateTime.Now.ToString();

        DateTime dateAfter = DateTime.ParseExact(dateBefore, "G", formatter, DateTimeStyles.AdjustToUniversal);
        Assert.Equal(dateBefore, dateAfter.ToString());
    }

    [Fact]
    public static void TestParseExact_String_StringArray_FormatProvider_DateTimeStyles()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString("g");

        var formats = new string[] { "g" };
        DateTime result = DateTime.ParseExact(expectedString, formats, null, DateTimeStyles.AdjustToUniversal);
        Assert.Equal(expectedString, result.ToString("g"));
    }

    [Fact]
    public static void TestTryParseExact_String_String_FormatProvider_DateTimeStyles_NullFormatProvider()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString("g");

        DateTime resulted;
        Assert.True(DateTime.TryParseExact(expectedString, "g", null, DateTimeStyles.AdjustToUniversal, out resulted));
        Assert.Equal(expectedString, resulted.ToString("g"));
    }

    [Fact]
    public static void TestTryParseExact_String_StringArray_FormatProvider_DateTimeStyles()
    {
        DateTime expected = DateTime.MaxValue;
        string expectedString = expected.ToString("g");

        var formats = new string[] { "g" };
        DateTime result;
        Assert.True(DateTime.TryParseExact(expectedString, formats, null, DateTimeStyles.AdjustToUniversal, out result));
        Assert.Equal(expectedString, result.ToString("g"));
    }

    public static void TestParseExact_EscapedSingleQuotes()
    {
        var formatInfo = DateTimeFormatInfo.GetInstance(new CultureInfo("mt-MT"));
        const string format = @"dddd, d' ta\' 'MMMM yyyy";

        DateTime expected = new DateTime(1999, 2, 28, 17, 00, 01);
        string formatted = expected.ToString(format, formatInfo);
        DateTime actual = DateTime.ParseExact(formatted, format, formatInfo);

        Assert.Equal(expected.Date, actual.Date);
    }

    [Theory]
    [InlineData("fi-FI")]
    [InlineData("nb-NO")]
    [InlineData("nb-SJ")]
    [InlineData("sr-Cyrl-XK")]
    [InlineData("sr-Latn-ME")]
    [InlineData("sr-Latn-RS")]
    [InlineData("sr-Latn-XK")]
    public static void TestParse_SpecialCultures(string cultureName)
    {
        // Test DateTime parsing with cultures which has the date separator and time separator are same
        CultureInfo cultureInfo;
        try
        {
            cultureInfo = new CultureInfo(cultureName);
        }
        catch (CultureNotFoundException)
        {
            // Ignore un-supported culture in current platform
            return;
        }

        var dateTime = new DateTime(2015, 11, 20, 11, 49, 50);
        string dateString = dateTime.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo);

        DateTime parsedDate;
        Assert.True(DateTime.TryParse(dateString, cultureInfo, DateTimeStyles.None, out parsedDate));
        if (cultureInfo.DateTimeFormat.ShortDatePattern.Contains("yyyy") || HasDifferentDateTimeSeparators(cultureInfo.DateTimeFormat))
        {
            Assert.Equal(dateTime.Date, parsedDate);
        }
        else
        {
            // When the date separator and time separator are the same, DateTime.TryParse cannot 
            // tell the difference between a short date like dd.MM.yy and a short time
            // like HH.mm.ss. So it assumes that if it gets 03.04.11, that must be a time
            // and uses the current date to construct the date time.
            DateTime now = DateTime.Now;
            Assert.Equal(new DateTime(now.Year, now.Month, now.Day, dateTime.Day, dateTime.Month, dateTime.Year % 100), parsedDate);
        }

        dateString = dateTime.ToString(cultureInfo.DateTimeFormat.LongDatePattern, cultureInfo);
        Assert.True(DateTime.TryParse(dateString, cultureInfo, DateTimeStyles.None, out parsedDate));
        Assert.Equal(dateTime.Date, parsedDate);

        dateString = dateTime.ToString(cultureInfo.DateTimeFormat.FullDateTimePattern, cultureInfo);
        Assert.True(DateTime.TryParse(dateString, cultureInfo, DateTimeStyles.None, out parsedDate));
        Assert.Equal(dateTime, parsedDate);

        dateString = dateTime.ToString(cultureInfo.DateTimeFormat.LongTimePattern, cultureInfo);
        Assert.True(DateTime.TryParse(dateString, cultureInfo, DateTimeStyles.None, out parsedDate));
        Assert.Equal(dateTime.TimeOfDay, parsedDate.TimeOfDay);
    }

    private static bool HasDifferentDateTimeSeparators(DateTimeFormatInfo dateTimeFormat)
    {
        // Since .NET Core doesn't expose DateTimeFormatInfo DateSeparator and TimeSeparator properties,
        // this method gets the separators using DateTime.ToString by passing in the invariant separators.
        // The invariant separators will then get turned into the culture's separators by ToString,
        // which are then compared.

        var dateTime = new DateTime(2015, 11, 24, 17, 57, 29);
        string separators = dateTime.ToString("/@:", dateTimeFormat);

        int delimiterIndex = separators.IndexOf('@');
        string dateSeparator = separators.Substring(0, delimiterIndex);
        string timeSeparator = separators.Substring(delimiterIndex + 1);
        return dateSeparator != timeSeparator;
    }

    [Fact]
    public static void TestGetDateTimeFormats()
    {
        var allStandardFormats = new char[]
        {
            'd', 'D', 'f', 'F', 'g', 'G',
            'm', 'M', 'o', 'O', 'r', 'R',
            's', 't', 'T', 'u', 'U', 'y', 'Y',
        };

        var dateTime = new DateTime(2009, 7, 28, 5, 23, 15);
        var formats = new List<string>();

        foreach (char format in allStandardFormats)
        {
            string[] dates = dateTime.GetDateTimeFormats(format);

            Assert.True(dates.Length > 0);

            DateTime parsedDate;
            Assert.True(DateTime.TryParseExact(dates[0], format.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedDate));

            formats.AddRange(dates);
        }

        List<string> actualFormats = dateTime.GetDateTimeFormats().ToList();
        Assert.Equal(formats.OrderBy(t => t), actualFormats.OrderBy(t => t));

        actualFormats = dateTime.GetDateTimeFormats(CultureInfo.CurrentCulture).ToList();
        Assert.Equal(formats.OrderBy(t => t), actualFormats.OrderBy(t => t));
    }

    [Fact]
    public static void TestGetDateTimeFormats_FormatSpecifier_InvalidFormat()
    {
        var dateTime = new DateTime(2009, 7, 28, 5, 23, 15);
        Assert.Throws<FormatException>(() => dateTime.GetDateTimeFormats('x')); // No such format
    }

    private static void VerifyDateTime(DateTime dateTime, int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
    {
        Assert.Equal(year, dateTime.Year);
        Assert.Equal(month, dateTime.Month);
        Assert.Equal(day, dateTime.Day);
        Assert.Equal(hour, dateTime.Hour);
        Assert.Equal(minute, dateTime.Minute);
        Assert.Equal(second, dateTime.Second);
        Assert.Equal(millisecond, dateTime.Millisecond);

        Assert.Equal(kind, dateTime.Kind);
    }

    private class MyFormatter : IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            return typeof(IFormatProvider) == formatType ? this : null;
        }
    }

    [Fact]
    public static void InvalidDateTimeStyles()
    {
        CultureInfo culture = new CultureInfo("en-US");
        string strDateTime = "Thursday, August 31, 2006 1:14";
        string[] formats = new string[] { "f" };
        IFormatProvider provider = new CultureInfo("en-US");
        DateTimeStyles style = DateTimeStyles.AssumeLocal | DateTimeStyles.AssumeUniversal;
        Assert.Throws<ArgumentException>(() => DateTime.ParseExact(strDateTime, formats, provider, style));
    }
}
