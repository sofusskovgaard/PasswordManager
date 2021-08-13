using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PasswordManager.Data.Entities.User;
using PasswordManager.Services.TokenService;
using PasswordManager.TestUtils;
using Xunit;
using Xunit.Abstractions;

namespace PasswordManager.Web.Tests.Pages
{
    public class Auth
    {
        private const string url = "https://localhost:5001";
        
        private readonly ITestOutputHelper output;

        public Auth(ITestOutputHelper output)
        {
            this.output = output;
        }
        
        [Fact]
        public void RedirectFromAccountToLoginIfNotAuthenticated()
        {
            using var driver = new ChromeDriver();

            driver.Navigate().GoToUrl(url+"/account");
            
            output.WriteLine($"Title: {driver.Title}");
            output.WriteLine($"Url: {driver.Url}");

            Assert.Equal(url+"/login", driver.Url);
            Assert.Equal("Login - PasswordManager", driver.Title);
            
            driver.Close();
        }
        
        [Fact]
        public void RegisterAccount()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(url+"/register");

            var expectedFirstname = "Tester";
            var expectedLastname = "McTest";
            var expectedEmail = "test.mctest@test.com";
            var expectedPassword = "TesterMcTestIsAwesome";
            
            driver.FindElement(By.Name("RegisterForm.Firstname")).SendKeys(expectedFirstname);
            driver.FindElement(By.Name("RegisterForm.Lastname")).SendKeys(expectedLastname);
            driver.FindElement(By.Name("RegisterForm.Email")).SendKeys(expectedEmail);
            driver.FindElement(By.Name("RegisterForm.Password")).SendKeys(expectedPassword);
            driver.FindElement(By.Name("RegisterForm.ConfirmPassword")).SendKeys(expectedPassword);
            
            driver.FindElement(By.TagName("button")).Click();

            var cookie = driver.Manage().Cookies.GetCookieNamed("Authorization");
            output.WriteLine($"Token: {cookie.Value}");

            Assert.NotNull(cookie);
            Assert.Equal("Authorization", cookie.Name);
            
            driver.Close();
        }
        
        [Fact]
        public void LoginWithAccount()
        {
            using IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(url+"/login");
            
            var expectedEmail = "test.mctest@test.com";
            var expectedPassword = "TesterMcTestIsAwesome";
            
            driver.FindElement(By.Name("LoginForm.Email")).SendKeys(expectedEmail);
            driver.FindElement(By.Name("LoginForm.Password")).SendKeys(expectedPassword);
            
            driver.FindElement(By.TagName("button")).Click();

            var cookie = driver.Manage().Cookies.GetCookieNamed("Authorization");
            output.WriteLine($"Token: {cookie.Value}");

            Assert.NotNull(cookie);
            Assert.Equal("Authorization", cookie.Name);
            
            driver.Close();
        }
    }
}