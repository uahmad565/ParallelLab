import React from 'react';
import '../styles/TermsOfService.css';

const TermsOfService: React.FC = () => {
  return (
    <div className="legal-page">
      <div className="legal-container">
        <h1>Terms of Service</h1>
        <p className="last-updated">Last Updated: {new Date().toLocaleDateString()}</p>

        <section>
          <h2>1. Acceptance of Terms</h2>
          <p>
            By accessing and using ParallelLab ("the Service"), you accept and agree to be bound by the
            terms and provision of this agreement. If you do not agree to abide by the above, please do
            not use this service.
          </p>
        </section>

        <section>
          <h2>2. Description of Service</h2>
          <p>
            ParallelLab is an educational platform that provides programming exercises, code execution
            services, and performance analysis tools for learning parallel programming concepts. The
            Service allows users to submit code, receive feedback, and compare their solutions with
            ideal implementations.
          </p>
        </section>

        <section>
          <h2>3. User Accounts</h2>
          <h3>3.1 Account Creation</h3>
          <p>To use certain features of the Service, you must register for an account. You agree to:</p>
          <ul>
            <li>Provide accurate, current, and complete information during registration</li>
            <li>Maintain and update your account information to keep it accurate</li>
            <li>Maintain the security of your password and account</li>
            <li>Accept responsibility for all activities under your account</li>
          </ul>

          <h3>3.2 Account Termination</h3>
          <p>
            We reserve the right to suspend or terminate your account at any time for violation of these
            Terms, fraudulent activity, or any other reason we deem necessary to protect the Service and
            its users.
          </p>
        </section>

        <section>
          <h2>4. Code Submissions and Intellectual Property</h2>
          <h3>4.1 Your Code</h3>
          <p>
            You retain ownership of any code you submit to the Service. By submitting code, you grant us
            a non-exclusive, worldwide, royalty-free license to use, store, execute, and analyze your
            code for the purpose of providing the Service.
          </p>

          <h3>4.2 Code Execution</h3>
          <p>
            Your code will be executed in a sandboxed environment. We are not responsible for any data
            loss, corruption, or security issues that may occur during code execution. You agree not to
            submit code that:
          </p>
          <ul>
            <li>Contains malicious software, viruses, or harmful code</li>
            <li>Attempts to access unauthorized resources or systems</li>
            <li>Violates any applicable laws or regulations</li>
            <li>Infringes on intellectual property rights of others</li>
          </ul>

          <h3>4.3 Our Content</h3>
          <p>
            All content provided by ParallelLab, including exercises, ideal solutions, and educational
            materials, is protected by copyright and other intellectual property laws. You may not
            reproduce, distribute, or create derivative works without our express written permission.
          </p>
        </section>

        <section>
          <h2>5. User Conduct</h2>
          <p>You agree not to:</p>
          <ul>
            <li>Use the Service for any illegal or unauthorized purpose</li>
            <li>Interfere with or disrupt the Service or servers</li>
            <li>Attempt to gain unauthorized access to any part of the Service</li>
            <li>Share your account credentials with others</li>
            <li>Use automated systems to submit code or manipulate the Service</li>
            <li>Harass, abuse, or harm other users</li>
            <li>Post or transmit any content that is offensive, defamatory, or violates any rights</li>
          </ul>
        </section>

        <section>
          <h2>6. Service Availability</h2>
          <p>
            We strive to provide continuous access to the Service but do not guarantee uninterrupted or
            error-free operation. The Service may be temporarily unavailable due to maintenance, updates,
            or circumstances beyond our control. We reserve the right to modify, suspend, or discontinue
            any part of the Service at any time.
          </p>
        </section>

        <section>
          <h2>7. Performance and Results</h2>
          <p>
            While we provide performance analysis and comparisons, results may vary based on system
            resources, network conditions, and other factors. We do not guarantee specific performance
            metrics or execution times. The Service is provided for educational purposes only.
          </p>
        </section>

        <section>
          <h2>8. Disclaimers</h2>
          <p>
            THE SERVICE IS PROVIDED "AS IS" AND "AS AVAILABLE" WITHOUT WARRANTIES OF ANY KIND, EITHER
            EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO IMPLIED WARRANTIES OF MERCHANTABILITY,
            FITNESS FOR A PARTICULAR PURPOSE, OR NON-INFRINGEMENT.
          </p>
          <p>
            We do not warrant that the Service will be uninterrupted, secure, or error-free, or that any
            defects will be corrected. We are not responsible for any loss or damage resulting from your
            use of the Service.
          </p>
        </section>

        <section>
          <h2>9. Limitation of Liability</h2>
          <p>
            TO THE MAXIMUM EXTENT PERMITTED BY LAW, PARALLELLAB SHALL NOT BE LIABLE FOR ANY INDIRECT,
            INCIDENTAL, SPECIAL, CONSEQUENTIAL, OR PUNITIVE DAMAGES, OR ANY LOSS OF PROFITS OR REVENUES,
            WHETHER INCURRED DIRECTLY OR INDIRECTLY, OR ANY LOSS OF DATA, USE, GOODWILL, OR OTHER
            INTANGIBLE LOSSES RESULTING FROM YOUR USE OF THE SERVICE.
          </p>
        </section>

        <section>
          <h2>10. Indemnification</h2>
          <p>
            You agree to indemnify and hold harmless ParallelLab, its officers, directors, employees,
            and agents from any claims, damages, losses, liabilities, and expenses (including legal fees)
            arising out of or relating to your use of the Service, violation of these Terms, or
            infringement of any rights of another.
          </p>
        </section>

        <section>
          <h2>11. Modifications to Terms</h2>
          <p>
            We reserve the right to modify these Terms at any time. We will notify users of material
            changes by posting the updated Terms on this page and updating the "Last Updated" date. Your
            continued use of the Service after such modifications constitutes acceptance of the updated
            Terms.
          </p>
        </section>

        <section>
          <h2>12. Governing Law</h2>
          <p>
            These Terms shall be governed by and construed in accordance with applicable laws, without
            regard to conflict of law provisions. Any disputes arising from these Terms or your use of
            the Service shall be resolved through appropriate legal channels.
          </p>
        </section>

        <section>
          <h2>13. Contact Information</h2>
          <p>
            If you have any questions about these Terms of Service, please contact us:
          </p>
          <ul>
            <li>Email: <a href="mailto:uahmad565565@gmail.com">uahmad565565@gmail.com</a></li>
            <li>Phone: <a href="tel:+923076331854">+92 307 6331854</a></li>
          </ul>
        </section>
      </div>
    </div>
  );
};

export default TermsOfService;

