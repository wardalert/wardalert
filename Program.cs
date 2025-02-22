using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using wardalert.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession();               // Add session services
builder.Services.AddRazorPages();            // Add Razor Pages services
builder.Services.AddScoped<TrainingService>();


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Enable session middleware
app.UseAuthorization();

app.MapRazorPages(); // Map Razor Pages endpoints

app.Run();

