using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using EduPr.Data;
using EduPr.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EduPr;

public partial class PatientAppealsControl : UserControl
{
    private readonly string[] _types =
    {
        "Жалоба",
        "Вопрос",
        "Предложение"
    };

    private readonly string[] _categories =
    {
        "Качество обслуживания",
        "Работа персонала",
        "Запись и расписание",
        "Оплата",
        "Условия клиники",
        "Другое"
    };

    private readonly string[] _statuses =
    {
        "Новое",
        "На рассмотрении",
        "Ожидает ответа",
        "Решено",
        "Отклонено",
        "Закрыто"
    };

    private List<PatientChoice> _patients = new();
    private List<UserChoice> _employees = new();

    private int? _editingId;
    private bool _initializing = true;
    private bool _ignoreGridSelection;

    public PatientAppealsControl()
    {
        InitializeComponent();

        SetupFilters();
        LoadLookups();
        ClearEditor();

        _initializing = false;

        LoadAppeals();
    }

    private void SetupFilters()
    {
        TypeFilter.ItemsSource =
            new[] { "Все типы" }.Concat(_types).ToList();

        CategoryFilter.ItemsSource =
            new[] { "Все категории" }.Concat(_categories).ToList();

        StatusFilter.ItemsSource =
            new[] { "Все статусы" }.Concat(_statuses).ToList();

        DeadlineFilter.ItemsSource = new[]
        {
            "Все сроки",
            "Просроченные",
            "Сегодня",
            "Следующие 7 дней",
            "Без срока"
        };

        TypeFilter.SelectedIndex = 0;
        CategoryFilter.SelectedIndex = 0;
        StatusFilter.SelectedIndex = 0;
        DeadlineFilter.SelectedIndex = 0;

        TypeComboBox.ItemsSource = _types;
        CategoryComboBox.ItemsSource = _categories;
        StatusComboBox.ItemsSource = _statuses;
    }

    private void LoadLookups()
    {
        try
        {
            using var db = new AppDbContext();

            _patients = db.Patients
                .AsNoTracking()
                .OrderBy(p => p.LastNamePatient)
                .ThenBy(p => p.FirstNamePatient)
                .ToList()
                .Select(p => new PatientChoice
                {
                    Id = p.IdPatient,
                    DisplayName = BuildPatientName(
                        p.LastNamePatient,
                        p.FirstNamePatient,
                        p.MiddleNamePatient,
                        p.IdPatient)
                })
                .ToList();

            _employees = db.Users
                .AsNoTracking()
                .OrderBy(u => u.LoginUser)
                .ToList()
                .Select(u => new UserChoice
                {
                    Id = u.IdUser,
                    DisplayName = u.LoginUser
                })
                .ToList();

            PatientComboBox.ItemsSource = _patients;
            ResponsibleComboBox.ItemsSource = _employees;
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text =
                $"Не удалось загрузить пациентов и сотрудников: {ex.Message}";
        }
    }

    private static string BuildPatientName(
        string? lastName,
        string? firstName,
        string? middleName,
        int id)
    {
        var fullName = string.Join(
            " ",
            new[] { lastName, firstName, middleName }
                .Where(s => !string.IsNullOrWhiteSpace(s)));

        return $"{fullName} (№{id})";
    }

    private void LoadAppeals()
    {
        try
        {
            using var db = new AppDbContext();

            var appeals = db.PatientAppeals
                .AsNoTracking()
                .Include(a => a.IdPatientNavigation)
                .Include(a => a.IdResponsibleUserNavigation)
                .OrderByDescending(a => a.CreatedAppeal)
                .ToList();

            var search = SearchBox.Text?.Trim() ?? "";

            if (!string.IsNullOrWhiteSpace(search))
            {
                appeals = appeals
                    .Where(a =>
                        (a.SubjectAppeal ?? "")
                            .Contains(search, StringComparison.OrdinalIgnoreCase)
                        ||
                        (a.DescriptionAppeal ?? "")
                            .Contains(search, StringComparison.OrdinalIgnoreCase)
                        ||
                        (a.IdPatientNavigation?.LastNamePatient ?? "")
                            .Contains(search, StringComparison.OrdinalIgnoreCase)
                        ||
                        (a.IdPatientNavigation?.FirstNamePatient ?? "")
                            .Contains(search, StringComparison.OrdinalIgnoreCase)
                        ||
                        (a.IdPatientNavigation?.PhonePatient ?? "")
                            .Contains(search, StringComparison.OrdinalIgnoreCase)
                        ||
                        (a.IdResponsibleUserNavigation?.LoginUser ?? "")
                            .Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var selectedType = TypeFilter.SelectedItem as string;
            if (!string.IsNullOrWhiteSpace(selectedType)
                && selectedType != "Все типы")
            {
                appeals = appeals
                    .Where(a => a.TypeAppeal == selectedType)
                    .ToList();
            }

            var selectedCategory = CategoryFilter.SelectedItem as string;
            if (!string.IsNullOrWhiteSpace(selectedCategory)
                && selectedCategory != "Все категории")
            {
                appeals = appeals
                    .Where(a => a.CategoryAppeal == selectedCategory)
                    .ToList();
            }

            var selectedStatus = StatusFilter.SelectedItem as string;
            if (!string.IsNullOrWhiteSpace(selectedStatus)
                && selectedStatus != "Все статусы")
            {
                appeals = appeals
                    .Where(a => a.StatusAppeal == selectedStatus)
                    .ToList();
            }

            var deadline = DeadlineFilter.SelectedItem as string;
            var today = DateTime.Today;

            switch (deadline)
            {
                case "Просроченные":
                    appeals = appeals
                        .Where(a =>
                            a.DueDateAppeal.HasValue
                            && a.DueDateAppeal.Value.Date < today
                            && a.StatusAppeal != "Решено"
                            && a.StatusAppeal != "Отклонено"
                            && a.StatusAppeal != "Закрыто")
                        .ToList();
                    break;

                case "Сегодня":
                    appeals = appeals
                        .Where(a =>
                            a.DueDateAppeal.HasValue
                            && a.DueDateAppeal.Value.Date == today)
                        .ToList();
                    break;

                case "Следующие 7 дней":
                    appeals = appeals
                        .Where(a =>
                            a.DueDateAppeal.HasValue
                            && a.DueDateAppeal.Value.Date >= today
                            && a.DueDateAppeal.Value.Date <= today.AddDays(7))
                        .ToList();
                    break;

                case "Без срока":
                    appeals = appeals
                        .Where(a => !a.DueDateAppeal.HasValue)
                        .ToList();
                    break;
            }

            var rows = appeals.Select(a => new AppealRow
            {
                IdAppeal = a.IdAppeal,
                CreatedText = a.CreatedAppeal.ToString("dd.MM.yyyy HH:mm"),
                PatientName = BuildPatientName(
                    a.IdPatientNavigation?.LastNamePatient,
                    a.IdPatientNavigation?.FirstNamePatient,
                    a.IdPatientNavigation?.MiddleNamePatient,
                    a.IdPatient),
                SubjectAppeal = a.SubjectAppeal,
                TypeAppeal = a.TypeAppeal,
                CategoryAppeal = a.CategoryAppeal,
                StatusAppeal = a.StatusAppeal,
                DueDateText = a.DueDateAppeal?.ToString("dd.MM.yyyy") ?? "—",
                ResponsibleName =
                    a.IdResponsibleUserNavigation?.LoginUser ?? "Не назначен"
            }).ToList();

            _ignoreGridSelection = true;
            AppealsGrid.ItemsSource = rows;
            _ignoreGridSelection = false;

            StatusTextBlock.Text = $"Найдено обращений: {rows.Count}.";
        }
        catch (Exception ex)
        {
            _ignoreGridSelection = false;
            StatusTextBlock.Text =
                $"Ошибка загрузки обращений: {ex.Message}";
        }
    }

    private void Search_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!_initializing)
            LoadAppeals();
    }

    private void Filter_SelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        if (!_initializing)
            LoadAppeals();
    }

    private void Refresh_Click(object? sender, RoutedEventArgs e)
    {
        LoadLookups();
        LoadAppeals();
    }

    private void NewAppeal_Click(object? sender, RoutedEventArgs e)
    {
        ClearEditor();
        StatusTextBlock.Text =
            "Заполните поля и нажмите «Сохранить обращение».";
    }

    private void ClearEditor()
    {
        _editingId = null;

        _ignoreGridSelection = true;
        AppealsGrid.SelectedItem = null;
        _ignoreGridSelection = false;

        PatientComboBox.SelectedItem = null;
        TypeComboBox.SelectedItem = "Жалоба";
        CategoryComboBox.SelectedItem = "Другое";
        StatusComboBox.SelectedItem = "Новое";
        ResponsibleComboBox.SelectedItem = null;

        SubjectTextBox.Text = "";
        DescriptionTextBox.Text = "";
        DueDatePicker.SelectedDate = null;

        HistoryList.ItemsSource = Array.Empty<PatientAppealHistory>();
        CommentTextBox.Text = "";
    }

    private void AppealsGrid_SelectionChanged(
        object? sender,
        SelectionChangedEventArgs e)
    {
        if (_initializing || _ignoreGridSelection)
            return;

        if (AppealsGrid.SelectedItem is AppealRow row)
            LoadEditor(row.IdAppeal);
    }

    private void LoadEditor(int id)
    {
        try
        {
            using var db = new AppDbContext();

            var appeal = db.PatientAppeals
                .AsNoTracking()
                .Include(a => a.IdPatientNavigation)
                .Include(a => a.IdResponsibleUserNavigation)
                .Include(a => a.History)
                    .ThenInclude(h => h.IdUserNavigation)
                .FirstOrDefault(a => a.IdAppeal == id);

            if (appeal == null)
            {
                StatusTextBlock.Text = "Обращение не найдено.";
                return;
            }

            _editingId = appeal.IdAppeal;

            PatientComboBox.SelectedItem =
                _patients.FirstOrDefault(p => p.Id == appeal.IdPatient);

            TypeComboBox.SelectedItem = appeal.TypeAppeal;
            CategoryComboBox.SelectedItem = appeal.CategoryAppeal;
            StatusComboBox.SelectedItem = appeal.StatusAppeal;

            ResponsibleComboBox.SelectedItem =
                _employees.FirstOrDefault(
                    u => u.Id == appeal.IdResponsibleUser);

            SubjectTextBox.Text = appeal.SubjectAppeal;
            DescriptionTextBox.Text = appeal.DescriptionAppeal;

            DueDatePicker.SelectedDate =
                appeal.DueDateAppeal.HasValue
                    ? new DateTimeOffset(appeal.DueDateAppeal.Value)
                    : null;

            HistoryList.ItemsSource = appeal.History
                .OrderByDescending(h => h.CreatedHistory)
                .ToList();

            CommentTextBox.Text = "";

            _ignoreGridSelection = true;
            if (AppealsGrid.ItemsSource is IEnumerable<AppealRow> rows)
            {
                AppealsGrid.SelectedItem =
                    rows.FirstOrDefault(r => r.IdAppeal == id);
            }
            _ignoreGridSelection = false;

            StatusTextBlock.Text = $"Открыто обращение №{id}.";
        }
        catch (Exception ex)
        {
            _ignoreGridSelection = false;
            StatusTextBlock.Text =
                $"Не удалось открыть обращение: {ex.Message}";
        }
    }

    private void SaveAppeal_Click(object? sender, RoutedEventArgs e)
    {
        if (PatientComboBox.SelectedItem is not PatientChoice patient)
        {
            StatusTextBlock.Text = "Выберите пациента.";
            return;
        }

        var subject = SubjectTextBox.Text?.Trim() ?? "";
        var description = DescriptionTextBox.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(subject))
        {
            StatusTextBlock.Text = "Введите тему обращения.";
            return;
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            StatusTextBlock.Text = "Введите описание обращения.";
            return;
        }

        var type = TypeComboBox.SelectedItem as string;
        var category = CategoryComboBox.SelectedItem as string;
        var status = StatusComboBox.SelectedItem as string;

        if (type == null || category == null || status == null)
        {
            StatusTextBlock.Text =
                "Выберите тип, категорию и статус обращения.";
            return;
        }

        var responsible = ResponsibleComboBox.SelectedItem as UserChoice;
        var dueDate = DueDatePicker.SelectedDate?.Date;

        try
        {
            using var db = new AppDbContext();

            var isNew = !_editingId.HasValue;

            PatientAppeal? appeal = null;

            if (!isNew)
            {
                appeal = db.PatientAppeals
                    .FirstOrDefault(a => a.IdAppeal == _editingId!.Value);

                if (appeal == null)
                {
                    StatusTextBlock.Text =
                        "Обращение не найдено. Обновите список.";
                    return;
                }
            }

            var oldStatus = appeal?.StatusAppeal;
            var oldResponsibleId = appeal?.IdResponsibleUser;

            if (appeal == null)
            {
                appeal = new PatientAppeal
                {
                    CreatedAppeal = DateTime.Now,
                    StatusAppeal = "Новое",
                    UpdatedAppeal = DateTime.Now
                };

                db.PatientAppeals.Add(appeal);
            }

            appeal.IdPatient = patient.Id;
            appeal.TypeAppeal = type;
            appeal.CategoryAppeal = category;
            appeal.SubjectAppeal = subject;
            appeal.DescriptionAppeal = description;
            appeal.StatusAppeal = status;
            appeal.IdResponsibleUser = responsible?.Id;
            appeal.DueDateAppeal = dueDate;
            appeal.UpdatedAppeal = DateTime.Now;

            db.SaveChanges();

            var userId = ContextData.CurrentUser?.IdUser;
            var historyEntries = new List<PatientAppealHistory>();

            if (isNew)
            {
                historyEntries.Add(new PatientAppealHistory
                {
                    IdAppeal = appeal.IdAppeal,
                    IdUser = userId,
                    ActionHistory = "Создано",
                    DetailsHistory = $"Зарегистрировано обращение: {subject}",
                    CreatedHistory = DateTime.Now
                });
            }
            else
            {
                historyEntries.Add(new PatientAppealHistory
                {
                    IdAppeal = appeal.IdAppeal,
                    IdUser = userId,
                    ActionHistory = "Данные обновлены",
                    DetailsHistory = "Сохранены изменения карточки обращения.",
                    CreatedHistory = DateTime.Now
                });

                if (oldStatus != appeal.StatusAppeal)
                {
                    historyEntries.Add(new PatientAppealHistory
                    {
                        IdAppeal = appeal.IdAppeal,
                        IdUser = userId,
                        ActionHistory = "Статус изменён",
                        DetailsHistory =
                            $"Старый статус: {oldStatus}. Новый статус: {appeal.StatusAppeal}.",
                        CreatedHistory = DateTime.Now
                    });
                }

                if (oldResponsibleId != appeal.IdResponsibleUser)
                {
                    historyEntries.Add(new PatientAppealHistory
                    {
                        IdAppeal = appeal.IdAppeal,
                        IdUser = userId,
                        ActionHistory = "Ответственный изменён",
                        DetailsHistory = responsible == null
                            ? "Ответственный сотрудник снят."
                            : $"Назначен ответственный: {responsible.DisplayName}.",
                        CreatedHistory = DateTime.Now
                    });
                }
            }

            db.PatientAppealHistories.AddRange(historyEntries);
            db.SaveChanges();

            var savedId = appeal.IdAppeal;

            LoadAppeals();
            LoadEditor(savedId);

            StatusTextBlock.Text =
                isNew
                    ? $"Обращение №{savedId} зарегистрировано."
                    : $"Обращение №{savedId} сохранено.";
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text =
                $"Не удалось сохранить обращение: {ex.Message}";
        }
    }

    private void AddComment_Click(object? sender, RoutedEventArgs e)
    {
        if (!_editingId.HasValue)
        {
            StatusTextBlock.Text =
                "Сначала выберите существующее обращение или сохраните новое.";
            return;
        }

        var comment = CommentTextBox.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(comment))
        {
            StatusTextBlock.Text = "Введите комментарий.";
            return;
        }

        try
        {
            using var db = new AppDbContext();

            var appealExists = db.PatientAppeals
                .Any(a => a.IdAppeal == _editingId.Value);

            if (!appealExists)
            {
                StatusTextBlock.Text = "Обращение не найдено.";
                return;
            }

            db.PatientAppealHistories.Add(new PatientAppealHistory
            {
                IdAppeal = _editingId.Value,
                IdUser = ContextData.CurrentUser?.IdUser,
                ActionHistory = "Комментарий",
                DetailsHistory = comment,
                CreatedHistory = DateTime.Now
            });

            db.SaveChanges();

            var savedId = _editingId.Value;
            LoadEditor(savedId);
            StatusTextBlock.Text = "Комментарий добавлен в историю.";
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text =
                $"Не удалось добавить комментарий: {ex.Message}";
        }
    }

    private sealed class PatientChoice
    {
        public int Id { get; set; }

        public string DisplayName { get; set; } = "";

        public override string ToString() => DisplayName;
    }

    private sealed class UserChoice
    {
        public int Id { get; set; }

        public string DisplayName { get; set; } = "";

        public override string ToString() => DisplayName;
    }

    private sealed class AppealRow
    {
        public int IdAppeal { get; set; }

        public string CreatedText { get; set; } = "";

        public string PatientName { get; set; } = "";

        public string SubjectAppeal { get; set; } = "";

        public string TypeAppeal { get; set; } = "";

        public string CategoryAppeal { get; set; } = "";

        public string StatusAppeal { get; set; } = "";

        public string DueDateText { get; set; } = "";

        public string ResponsibleName { get; set; } = "";
    }
}