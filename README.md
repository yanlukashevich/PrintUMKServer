# DrukMruk — early prototype

> ⚠️ **This repository is an early prototype, no longer maintained.**
> It was my first attempt at the system that later became **[drukmruk.pl](https://drukmruk.pl)** — a self-service print kiosk network now running in production. The production codebase is a separate, private, commercial project; this repo is kept public only to show where the idea started. The Azure resources it once referenced have been deleted.

---

## What is DrukMruk?

DrukMruk is a network of self-service print stations for university campuses. A student uploads a file from their phone or laptop, pays online, and receives a 6-digit pickup code. They walk up to any DrukMruk station, type the code on a touch screen, and collect the print — no account, no app, no queue, available at any hour.

The idea came from a real problem: a student survey showed that ~80% of students print regularly (notes, study materials) but find the existing options inconvenient — long queues, limited opening hours, having to leave campus.

**Live product:** [drukmruk.pl](https://drukmruk.pl)

## How it works (student flow)

1. **Open drukmruk.pl** in a browser — no sign-up, no login.
2. **Upload a file** (PDF, Word doc, image) and pick options: copies, single/double-sided, black & white or colour. The server computes the price from the page count.
3. **Pay online** through the Autopay payment gateway (BLIK, card, bank transfer).
4. **Get a 6-digit pickup code**, valid for 5 days.
5. **Collect the print** at any DrukMruk station by typing the code on the touch screen. The file is deleted the moment it's printed. If the print isn't collected within 5 days, the payment is refunded automatically and the file is erased.

## Production architecture (the real system)

The production system — which lives in a private repository — is a distributed application spanning three cooperating parts:

- **Web app** — React 19 SPA. A fully anonymous, account-free flow: upload, print options, payment, pickup codes.
- **Cloud backend** — ASP.NET Core 8 API on Azure. Owns the business logic: server-side pricing, the full **Autopay payment integration** (payment initiation, cryptographically verified webhooks, and **automatic refunds** for prints that are never collected), pickup-code generation, the print-job queue, and kiosk coordination.
- **Print stations (kiosks)** — Raspberry Pi 5 + touch display + IPP printer in a locked enclosure, running as a single-purpose terminal. A local daemon pulls the job once a code is entered, prints it, and never persists user files. Each station runs over its own LTE connection, fully isolated from the university network.

Running on Microsoft Azure (Poland Central) with a least-privilege security model: passwordless service-to-service authentication (Managed Identity), secrets in Key Vault, and a network-isolated database and file storage.

**Tech stack:** React 19 · Vite · Tailwind · ASP.NET Core 8 · Entity Framework Core · Azure (App Service, SQL, Blob Storage, Key Vault, Application Insights) · Python / FastAPI · CUPS / IPP · Raspberry Pi · Autopay payment gateway.

---

## About this repository

This is the **original 2026 prototype**, written as a coursework project and the proof of concept that started everything. It is intentionally minimal: an ASP.NET Core MVC app that explored the core idea of the architecture — **upload a file → store it in Azure Blob Storage → push a job onto an Azure Service Bus queue → have a printer pick it up.** It uses ASP.NET Identity for accounts and stops short of payments and pickup codes.

It is full of `// TODO` markers and rough edges — that's the point. Everything here was later redesigned and rewritten from scratch for the production system: anonymous sessions instead of user accounts, a real payment and refund flow, the physical kiosk fleet, and a proper cloud security model.

**Status: archived / dead.** Not maintained, not deployable as-is. Kept public as a portfolio artifact showing the starting point of a project that shipped.

## License

All rights reserved. See [LICENSE](LICENSE). This code is published for portfolio/reference purposes only.