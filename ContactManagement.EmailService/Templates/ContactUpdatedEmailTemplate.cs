using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.EmailService.Templates
{
    public static class ContactUpdatedEmailTemplate
    {
        public static string GetTemplate(string contactName)
        {
            return $@"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <h2 style='color: #333;'>Contact Information Updated</h2>
                        <p>Hello {contactName},</p>
                        <p>This email is to confirm that your contact information has been successfully updated in our system.</p>
                        <p>If you did not make these changes, please contact our support team immediately.</p>
                        <p style='margin-top: 20px;'>
                            Best regards,<br>
                            The Contact Management Team
                        </p>
                    </div>
                </body>
            </html>";
        }
    }

}
