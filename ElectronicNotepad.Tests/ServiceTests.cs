using NUnit.Framework;
using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;
using ElectronicNotepad.Core.Enums;
using ElectronicNotepad.Core.Services;
using System;
using System.Linq;

namespace ElectronicNotepad.Tests;

[TestFixture]
public class ValidationServiceTests
{
    private ValidationService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new ValidationService();
    }

    [Test]
    public void ValidateNote_EmptyTitle_ReturnsError()
    {
        var note = new Note { Title = "" };
        var errors = _service.ValidateNote(note).ToList();
        Assert.That(errors, Contains.Item("Заголовок нотатки не може бути порожнім."));
    }

    [Test]
    public void ValidateNote_TitleTooLong_ReturnsError()
    {
        var note = new Note { Title = new string('A', 101) };
        var errors = _service.ValidateNote(note).ToList();
        Assert.That(errors, Contains.Item("Заголовок занадто довгий (макс. 100 символів)."));
    }

    [Test]
    public void ValidateReminder_MessageEmpty_ReturnsError()
    {
        var reminder = new Reminder { Message = "" };
        var errors = _service.ValidateReminder(reminder).ToList();
        Assert.That(errors, Contains.Item("Текст нагадування не може бути порожнім."));
    }

    [Test]
    public void ValidateReminder_PastTime_ReturnsError()
    {
        var reminder = new Reminder { Message = "Test", ReminderTime = DateTime.Now.AddHours(-1) };
        var errors = _service.ValidateReminder(reminder).ToList();
        Assert.That(errors, Contains.Item("Час нагадування не може бути в минулому."));
    }
    
    [Test]
    public void ValidateNote_ValidNote_NoErrors()
    {
        var note = new Note { Title = "Normal Title", Content = "Some content" };
        var errors = _service.ValidateNote(note).ToList();
        Assert.That(errors.Count, Is.EqualTo(0));
    }
}

[TestFixture]
public class ExportServiceTests
{
    private ExportService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new ExportService();
    }

    [Test]
    public void ExportNote_TextFormat_ContainsTitle()
    {
        var note = new Note { Title = "MyTitle", Content = "MyContent" };
        var result = _service.ExportNote(note, ExportFormat.Text);
        Assert.That(result, Does.Contain("MYTITLE"));
        Assert.That(result, Does.Contain("MyContent"));
    }

    [Test]
    public void ExportNote_MarkdownFormat_HasHeader()
    {
        var note = new Note { Title = "MyTitle", Content = "MyContent" };
        var result = _service.ExportNote(note, ExportFormat.Markdown);
        Assert.That(result, Does.StartWith("# MyTitle"));
    }

    [Test]
    public void ExportNote_HtmlFormat_HasBodyTags()
    {
        var note = new Note { Title = "MyTitle", Content = "MyContent" };
        var result = _service.ExportNote(note, ExportFormat.Html);
        Assert.That(result, Does.Contain("<html"));
        Assert.That(result, Does.Contain("<body"));
        Assert.That(result, Does.Contain("<h1>MyTitle</h1>"));
    }
}
