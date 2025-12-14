import React from 'react';
import '../styles/PrivacyPolicy.css';

const PrivacyPolicy: React.FC = () => {
  return (
    <div className="legal-page">
      <div className="legal-container">
        <h1>Privacy Policy</h1>
        <p className="last-updated">Last Updated: {new Date().toLocaleDateString()}</p>

        <section>
          <h2>1. Introduction</h2>
          <p>
            Welcome to ParallelLab ("we," "our," or "us"). We are committed to protecting your privacy
            and ensuring you have a positive experience on our website. This Privacy Policy explains how
            we collect, use, disclose, and safeguard your information when you visit our website and use
            our services.
          </p>
        </section>

        <section>
          <h2>2. Information We Collect</h2>
          <h3>2.1 Information You Provide</h3>
          <p>We collect information that you provide directly to us, including:</p>
          <ul>
            <li>Account registration information (username, email, password)</li>
            <li>Code submissions and exercise solutions</li>
            <li>Contact information when you reach out to us</li>
            <li>Feedback and communications</li>
          </ul>

          <h3>2.2 Automatically Collected Information</h3>
          <p>When you use our services, we automatically collect certain information, including:</p>
          <ul>
            <li>IP address and device information</li>
            <li>Browser type and version</li>
            <li>Usage data and interaction patterns</li>
            <li>Cookies and similar tracking technologies</li>
          </ul>
        </section>

        <section>
          <h2>3. How We Use Your Information</h2>
          <p>We use the information we collect to:</p>
          <ul>
            <li>Provide, maintain, and improve our services</li>
            <li>Process and evaluate code submissions</li>
            <li>Track your progress and performance</li>
            <li>Send you updates and notifications</li>
            <li>Respond to your inquiries and support requests</li>
            <li>Detect and prevent fraud or abuse</li>
            <li>Comply with legal obligations</li>
          </ul>
        </section>

        <section>
          <h2>4. Cookies and Tracking Technologies</h2>
          <p>
            We use cookies and similar tracking technologies to track activity on our website and hold
            certain information. Cookies are files with a small amount of data which may include an
            anonymous unique identifier. You can instruct your browser to refuse all cookies or to
            indicate when a cookie is being sent.
          </p>
          <p>
            We use both session cookies (which expire when you close your browser) and persistent cookies
            (which stay on your device until deleted or expired).
          </p>
        </section>

        <section>
          <h2>5. Third-Party Services</h2>
          <h3>5.1 Google AdSense</h3>
          <p>
            Our website uses Google AdSense, a service provided by Google LLC. Google AdSense uses cookies
            and similar technologies to serve personalized advertisements based on your browsing behavior.
            Google may collect and process information about your visits to this and other websites to
            provide relevant ads.
          </p>
          <p>
            For more information about how Google uses data, please visit:
            <a href="https://policies.google.com/privacy" target="_blank" rel="noopener noreferrer">
              Google Privacy Policy
            </a>
          </p>
          <p>
            You can opt out of personalized advertising by visiting:
            <a href="https://www.google.com/settings/ads" target="_blank" rel="noopener noreferrer">
              Google Ad Settings
            </a>
          </p>

          <h3>5.2 Other Third-Party Services</h3>
          <p>
            We may use other third-party services for analytics, hosting, and functionality. These services
            may collect information about your use of our website in accordance with their own privacy
            policies.
          </p>
        </section>

        <section>
          <h2>6. Data Security</h2>
          <p>
            We implement appropriate technical and organizational security measures to protect your
            personal information against unauthorized access, alteration, disclosure, or destruction.
            However, no method of transmission over the Internet or electronic storage is 100% secure.
          </p>
        </section>

        <section>
          <h2>7. Your Rights</h2>
          <p>Depending on your location, you may have the following rights regarding your personal information:</p>
          <ul>
            <li>Right to access your personal data</li>
            <li>Right to rectify inaccurate data</li>
            <li>Right to erasure ("right to be forgotten")</li>
            <li>Right to restrict processing</li>
            <li>Right to data portability</li>
            <li>Right to object to processing</li>
            <li>Right to withdraw consent</li>
          </ul>
          <p>
            To exercise these rights, please contact us at{' '}
            <a href="mailto:uahmad565565@gmail.com">uahmad565565@gmail.com</a>.
          </p>
        </section>

        <section>
          <h2>8. Data Retention</h2>
          <p>
            We retain your personal information for as long as necessary to fulfill the purposes outlined
            in this Privacy Policy, unless a longer retention period is required or permitted by law.
            When we no longer need your information, we will securely delete or anonymize it.
          </p>
        </section>

        <section>
          <h2>9. Children's Privacy</h2>
          <p>
            Our services are not intended for children under the age of 13. We do not knowingly collect
            personal information from children under 13. If you believe we have collected information from
            a child under 13, please contact us immediately.
          </p>
        </section>

        <section>
          <h2>10. Changes to This Privacy Policy</h2>
          <p>
            We may update this Privacy Policy from time to time. We will notify you of any changes by
            posting the new Privacy Policy on this page and updating the "Last Updated" date. You are
            advised to review this Privacy Policy periodically for any changes.
          </p>
        </section>

        <section>
          <h2>11. Contact Us</h2>
          <p>
            If you have any questions about this Privacy Policy or our data practices, please contact us:
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

export default PrivacyPolicy;

