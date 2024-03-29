﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Models;
public class BookModel
{
    public int BookId { get; set; }

    public int UserId { get; set; }

    [Display(Name = "Book Name")]
    public string BookName { get; set; }

    [Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Chapters")]
    public int? Chapters { get; set; }

    [Display(Name = "Pages")]
    public int? Pages { get; set; }

    [Display(Name = "Notes")]
    public string? BookNotes { get; set; }

    public virtual string MobileView => $"{BookName}" +
        $"{(StartDate != null ? $"<br>{StartDate.Value.ToString("MM/dd/yyyy")}" : "")}" +
        $"{(EndDate != null ? $"<br>{EndDate.Value.ToString("MM/dd/yyyy")}" : "")}" +
        $"{(Chapters != null ? $"<br>Chapters: {Chapters}" : "")}" +
        $"{(Pages != null ? $"<br>Pages: {Pages}" : "")}" +
        $"{(!string.IsNullOrEmpty(BookNotes) ? $"<br>{BookNotes}" : "")}";
}

public class YearStatsBookModel
{
    [Display(Name = "User Id")]
    public int UserId { get; set; }

    [Display(Name = "Name")]
    public string Name { get; set; }

    [Display(Name = "Year")]
    public int Year { get; set; }

    [Display(Name = "Books")]
    public int BookCnt { get; set; }

    [Display(Name = "Chapters")]
    public int ChapterCnt { get; set; }

    [Display(Name = "Pages")]
    public int PageCnt { get; set; }

    [Display(Name = "Total Days")]
    public decimal TotalDays { get; set; }

    [Display(Name = "Average Days")]
    public decimal DayAvg => TotalDays / BookCnt;

    [Display(Name = "Average Days")]
    public string DayAvgZ => $"{Math.Round(DayAvg, 2):0.##}";

    [Display(Name = "Average Chapters")]
    public decimal ChapterAvg => (decimal)ChapterCnt / BookCnt;

    [Display(Name = "Average Chapters")]
    public string ChapterAvgZ => $"{Math.Round(ChapterAvg, 2):0.##}";

    [Display(Name = "Average Pages")]
    public decimal PageAvg => (decimal)PageCnt / BookCnt;

    [Display(Name = "Average Pages")]
    public string PageAvgZ => $"{Math.Round(PageAvg, 2):0.##}";

    public virtual string MobileView => $"{Year}" +
        $"<br>{Name}" +
        $"<br>Books: {BookCnt}" +
        $"<br>Chapters: {ChapterCnt}" +
        $"<br>Pages: {PageCnt}" +
        $"<br>Average Days: {DayAvgZ}" +
        $"<br>Average Chapters: {ChapterAvgZ}" +
        $"<br>Average Pages: {PageAvgZ}";
}
