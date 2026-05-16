using System;
using System.Linq;
using System.Text;
using ElectronicNotepad.Core.Enums;
using ElectronicNotepad.Core.Interfaces;
using ElectronicNotepad.Core.Models;

namespace ElectronicNotepad.Core.Services;

public class ExportService : IExportService
{
    private const string HtmlTemplate = @"
<!DOCTYPE html>
<html lang='uk'>
<head>
    <meta charset='UTF-8'>
    <style>
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; max-width: 800px; margin: 40px auto; padding: 20px; background-color: #f7f9f7; }
        .note-container { background: white; padding: 30px; border-radius: 15px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); border-left: 8px solid #a8d5ba; }
        h1 { color: #2f4f4f; margin-bottom: 5px; }
        .meta { color: #666; font-size: 0.9em; margin-bottom: 20px; border-bottom: 1px solid #eee; padding-bottom: 10px; }
        .content { white-space: pre-wrap; margin-bottom: 30px; }
        .reminders { background: #f0f8f0; padding: 15px; border-radius: 8px; }
        .reminders h3 { margin-top: 0; color: #4a6a4a; }
        ul { padding-left: 20px; }
        li { margin-bottom: 8px; }
    </style>
</head>
<body>
    <div class='note-container'>
        <h1>{{Title}}</h1>
        <div class='meta'>Створено: {{CreatedAt}} | Пріоритет: {{Priority}}</div>
        <div class='content'>{{Content}}</div>
        {{RemindersSection}}
    </div>
</body>
</html>";

    public string ExportNote(Note note, ExportFormat format)
    {
        return format switch
        {
            ExportFormat.Markdown => ExportAsMarkdown(note),
            ExportFormat.Html => ExportAsHtml(note),
            _ => ExportAsText(note)
        };
    }

    private string ExportAsText(Note note)
    {
        var sb = new StringBuilder();
        sb.AppendLine("========================================");
        sb.AppendLine($"  {note.Title.ToUpper()}");
        sb.AppendLine("========================================");
        sb.AppendLine($"Дата створення: {note.CreatedAt:dd.MM.yyyy HH:mm}");
        sb.AppendLine($"Пріоритет:      {note.Priority}");
        sb.AppendLine("----------------------------------------");
        sb.AppendLine();
        sb.AppendLine(note.Content);
        sb.AppendLine();
        
        if (note.Reminders.Any())
        {
            sb.AppendLine("----------------------------------------");
            sb.AppendLine("НАГАДУВАННЯ:");
            foreach (var r in note.Reminders)
            {
                string status = r.IsCompleted ? "[ВИКОНАНО]" : "[АКТИВНЕ]";
                sb.AppendLine($"- {r.ReminderTime:dd.MM.yyyy HH:mm} {status}: {r.Message}");
            }
        }
        sb.AppendLine("========================================");
        return sb.ToString();
    }

    private string ExportAsMarkdown(Note note)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# {note.Title}");
        sb.AppendLine($"> **Створено:** {note.CreatedAt:dd.MM.yyyy HH:mm}  ");
        sb.AppendLine($"> **Пріоритет:** `{note.Priority}`");
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine(note.Content);
        sb.AppendLine();
        
        if (note.Reminders.Any())
        {
            sb.AppendLine("---");
            sb.AppendLine("### 🔔 Нагадування");
            foreach (var r in note.Reminders)
            {
                string checkbox = r.IsCompleted ? "[x]" : "[ ]";
                sb.AppendLine($"{checkbox} **{r.ReminderTime:dd.MM.yyyy HH:mm}**: {r.Message}");
            }
        }
        return sb.ToString();
    }

    private string ExportAsHtml(Note note)
    {
        string remindersSection = "";
        if (note.Reminders.Any())
        {
            var rSb = new StringBuilder();
            rSb.AppendLine("<div class='reminders'><h3>🔔 Нагадування</h3><ul>");
            foreach (var r in note.Reminders)
            {
                string style = r.IsCompleted ? "style='text-decoration: line-through; color: #888;'" : "";
                rSb.AppendLine($"<li {style}><b>{r.ReminderTime:dd.MM.yyyy HH:mm}</b>: {r.Message}</li>");
            }
            rSb.AppendLine("</ul></div>");
            remindersSection = rSb.ToString();
        }

        return HtmlTemplate
            .Replace("{{Title}}", note.Title)
            .Replace("{{CreatedAt}}", note.CreatedAt.ToString("dd.MM.yyyy HH:mm"))
            .Replace("{{Priority}}", note.Priority.ToString())
            .Replace("{{Content}}", note.Content.Replace("\n", "<br/>"))
            .Replace("{{RemindersSection}}", remindersSection);
    }
}
