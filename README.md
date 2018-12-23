## What is this branch?

Reloaded is splitting up.

Peaking over 50K lines of code, Reloaded has become a too large of a project for me to handle alone in one monolithic repository handling all of the individual libraries, stock mods and other components such as plugins.

As a result, to ensure quality control and to ensure the project is more manageable, I'm splitting up the project.

In the future, the Reloaded libraries, samples and mods will all be slightly refactored, improved and available in their small, standalone repositories.

Specifically, the contents of this singular repository is currently being rebuilt into the following Github organization: https://github.com/Reloaded-Project ; starting with the individual libReloaded libraries.

The goal

## What (really) is this branch?

Quite simply, this is a fork of the master branch where I remove code little by little every time I implement something new into the Reloaded-Project organization.

Whether the project builds or not does not matter, it's just an indicator of progress of how much I still have to port and refactor.

In other words; I am taking the bricks of one large house and using them to make many small houses.

## The Future

Reloaded will be ported to .NET Core 3.0, inevitably with all of its libraries being available in .NET Standard fashion.

This means that main mod development will be happening on .NET Core in order to reap its benefits. That doesn't mean that .NET Framework development will not be possible or that existing mods would break - it's more of the matter of fact that .NET Core is just more cutting edge and seeping with performance improvements.