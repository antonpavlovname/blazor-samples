namespace BlazorServerSignalRApp.Data;

public record TestItem(int Id, string Name);
public record ComplexSampleRequest(string Title, string Description, List<TestItem> Items);
public record ComplexSampleResponse(bool Success, TestItem? SelectedItem);