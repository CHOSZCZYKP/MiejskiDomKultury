using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Stripe.Checkout;
using Stripe.Forwarding;

namespace MiejskiDomKultury.Services
{
    public class StripeService
    {
        string apiKey = Environment.GetEnvironmentVariable("STRIPE_KEY");
        public string CreateCheckoutSession(long kwota)
        {
            var client = new Stripe.StripeClient(apiKey);

            var lineItems = new List<SessionLineItemOptions>
    {
        new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = kwota * 100,
                Currency = "pln",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = $"Płatność za usługę w Ostrołęckim Domu Kultury"
                }
            },
            Quantity = 1
        }
    };

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card", "klarna", "blik" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "http://localhost:58741/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "http://localhost:58741/cancel?session_id={CHECKOUT_SESSION_ID}",
            };

            var service = new SessionService(client);
            Session session = service.Create(options);

           
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = session.Url,
                UseShellExecute = true
            });

            return session.Id; 
        }

        public void Success()
        {
            
            System.Windows.MessageBox.Show("Płatność zakończona sukcesem!");
        }

        public void Cancel()
        {
            
            System.Windows.MessageBox.Show("Płatność anulowana.");
        }


      
    }
}
