using System;

public class TodoItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Title { get; set; }
    public int Priority { get; set; } = 1;
    public bool IsDone { get; set; } = false;
}