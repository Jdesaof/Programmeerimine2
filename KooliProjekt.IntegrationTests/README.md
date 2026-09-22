# 19.02 integration tests

Based on the teacher's TestApplicationFactory / FakeStartup / TestBase structure.
The 73 tests use actual HTTP endpoints, validators, MediatR behaviors, repositories
and SQL Server LocalDB. There are no repository mocks or EF InMemory providers.

Run `./run-tests.ps1` from this directory, or use Test Explorer after opening
the root `KooliProjekt.sln`. Requires .NET 9 SDK, NuGet access and the existing
LocalDB instance `OlleProjekt`.

Each run creates a GUID-named `KooliProjekt_IT_...` database on that instance.
The fixture verifies its exact generated name before resetting or deleting it.
The normal application database is not used. The test collection is sequential;
each test clears only the owned database and creates its own parent records.
Normal completion deletes the owned database. An interrupted process may leave
that temporary database behind.

Covered: list/get, absent records, creation, update, invalid creation/update,
missing parents, deletion/repeated deletion, invalid IDs, and SQL cascade deletion.
Database assertions use fresh DI scopes to check committed state.

Contract difference from the teacher's sample: saving a missing record returns
404 in this application, while the sample expects 400. These tests preserve 404.
Routes also follow this application: POST /api/Olud and DELETE /api/Olud/{id}.

These tests are separate from the 497 unit tests and their coverage report.
