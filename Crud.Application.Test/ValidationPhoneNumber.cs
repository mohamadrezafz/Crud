using Crud.Application.Common.Extentions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Application.Tests
{
    public class ValidationPhoneNumber
    {
        public ValidationPhoneNumber()
        {
                
        }
        [Theory]
        [InlineData("+989120259301")]
        public  void  Valid_PhoneNumber_Should_Pass(string phoneNumber)
        {
            var validation = Utilities.IsPhoneNumberValid(phoneNumber);
            validation.Should().BeTrue();
        }

        [Theory]
        [InlineData("+982188776655")]
        public void Valid_PhoneNumber_Should_Fail(string phoneNumber)
        {
            var validation = Utilities.IsPhoneNumberValid(phoneNumber);
            validation.Should().BeFalse();
        }
    }
}
