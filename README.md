# JiuManager

Sistema de gestão para equipes de Jiu-Jitsu, em ASP.NET Core MVC (.NET 10), C#, MySQL 8.4, Dapper, Razor, Bootstrap 5, Font Awesome, Nunito e JavaScript puro.

## Executar

Pré-requisitos: .NET SDK 10 e Docker Desktop ativo (ou um servidor MySQL configurado).

No PowerShell, dentro desta pasta:

~~~powershell
docker compose up -d --wait
dotnet restore
dotnet run
~~~

Acesse **http://localhost:5080**.

- E-mail: **admin@admin.com**
- Senha inicial: **admin**
- Alteração de senha: clique no avatar no canto superior direito.

O banco, tabelas, índices, relacionamentos, permissões e dados iniciais são criados automaticamente na primeira execução. Reiniciar preserva os registros. O volume Docker `jiu-manage_dados_mysql` mantém os dados entre reinícios.

## Configuração

`appsettings.json` contém uma conexão exclusiva para desenvolvimento local:

~~~text
Server=127.0.0.1;Port=3306;Database=jiumanager;User ID=jiu;Password=jiu_local_2026;Allow User Variables=true
~~~

Para outro MySQL, use a variável de ambiente `ConnectionStrings__Banco` ou Secret Manager. O usuário do banco precisa de permissões para criar o banco e as tabelas. As credenciais do compose são somente para o ambiente local; configure credenciais próprias e HTTPS antes de publicar.

- `Dados:Demonstracao`: ativa a carga inicial fictícia, somente quando ainda não há equipe. Defina como `false` antes da primeira inicialização para começar sem alunos e professores fictícios.
- `Publico:UrlBase`: endereço público usado pelo QR Code. No ambiente local usa localhost; configure o domínio real quando publicar para permitir a leitura em celulares.
- `AllowedHosts`: ajuste para o domínio da implantação.
- Uploads: `wwwroot/uploads`, imagens JPG, PNG e WebP de até 3 MB, com validação de assinatura e nome gerado.

## Funcionalidades

- Dashboard com dados do MySQL, distribuição por faixa, evolução de ingressantes, previsão de graduações, mensalidades e agenda.
- Cadastros de equipe, unidades, professores, faixas, graus, alunos, turmas e matrículas.
- Perfil do aluno com dados, linha do tempo, frequência, financeiro e participações em campeonatos.
- Graduação transacional: valida progressão, mantém faixa/grau anterior, atualiza aluno e registra histórico imutável.
- Chamada por turma e data: permite corrigir a chamada sem duplicar frequência. Só aceita alunos matriculados.
- Campeonatos, inscrições, categorias, resultados e eventos.
- Planos, geração mensal em lote, mensalidades, recebimentos parciais e totais, saldo e inadimplência calculada por vencimento.
- Carteirinha com QR Code local (sem serviço externo) e validação pública por código aleatório.
- Certificado de graduação com registro de emissão e layout para impressão / salvar como PDF pelo navegador.
- Relatórios por faixa, graduação, frequência, financeiro e atletas, com CSV protegido contra fórmulas.
- Usuários, perfis e permissões adicionais, verificados no servidor.

Administrador tem acesso total. Professor acessa alunos, turmas, matrículas, frequência, graduação e campeonatos. Secretaria acessa alunos, professores, planos, financeiro e relatórios. Aluno acessa somente seu próprio perfil e carteirinha. Para um usuário Aluno, selecione seu vínculo no cadastro do usuário. As permissões adicionais ampliam os perfis Professor e Secretaria; não ampliam o perfil Aluno.

O filtro de unidade no topo afeta dashboard, alunos, professores e relatórios. A previsão de graduação é uma estimativa pelo intervalo configurado da faixa; a decisão de graduar é do professor. A evolução de alunos usa datas de ingresso acumuladas, não snapshots históricos de situação.

A estrutura usa `EquipeId` nas consultas e relações validadas dentro da equipe para permitir evolução futura. O cadastro e provisionamento comercial de múltiplas equipes/SaaS não faz parte desta versão.

## Organização

- `Controllers`: requisições MVC e respostas.
- `Models`: entidades com validações; `Models/ViewModels`: modelos de tela.
- `Interfaces`: contratos de persistência.
- `Repositories`: SQL parametrizado, consultas Dapper e transações MySQL.
- `Services`: regras de negócio, autenticação, permissões e composição das telas.
- `Dados`: inicialização, esquema e carga inicial transacional.
- `Configuracoes/modulos.json`: metadados internos dos campos e módulos; nomes SQL vêm somente deste catálogo do servidor.
- `Views`: pastas próprias por módulo e componentes Razor reutilizáveis em Shared.
- `Utils` e `Helpers`: validação, QR Code, hash, formatação e integração MVC.

Não usa Entity Framework, jQuery, Angular, React, Vue ou Tailwind. Bootstrap, Nunito e Font Awesome são servidos localmente, sem dependência de CDN durante o uso (ver Licenças).

## Validação

~~~powershell
dotnet build
node Testes/verificar-paginas.cjs
~~~

O teste de páginas usa `http://localhost:5080`, autentica como administrador e verifica páginas, relatórios, bloqueio anônimo e CSRF, sem modificar cadastros.

`Testes/verificar-fluxos.cjs` só funciona na instância **isolada** de `localhost:5081`. Cria registros de teste; nunca execute apontando para dados reais. Para preparar o banco:

~~~powershell
docker compose exec -T banco mysql -uroot -praiz_local_2026 -e "CREATE DATABASE IF NOT EXISTS jiumanager_teste CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci; GRANT ALL PRIVILEGES ON jiumanager_teste.* TO 'jiu'@'%';"
$env:ConnectionStrings__Banco = 'Server=127.0.0.1;Port=3306;Database=jiumanager_teste;User ID=jiu;Password=jiu_local_2026;Allow User Variables=true'
dotnet run --no-launch-profile --urls http://localhost:5081
~~~

Em outro terminal, na pasta do projeto:

~~~powershell
node Testes/verificar-fluxos.cjs
~~~

Esse teste verifica gravação e vínculos, valores decimais, chamada e percentual, graduação e imutabilidade, pagamento parcial e total, bloqueio de excesso e duplicidade, perfis, portal individual, validação pública, QR Code, certificado, filtro de unidade e troca de senha.

## Licenças de recursos visuais

- Bootstrap: MIT, licença incluída em `wwwroot/lib/bootstrap`.
- Font Awesome Free: ícones CC BY 4.0, fontes SIL OFL 1.1 e código MIT — https://fontawesome.com/license/free
- Nunito: SIL Open Font License — https://fonts.google.com/specimen/Nunito/license

## Encerrar

Encerre a aplicação com Ctrl+C. Para parar somente o banco deste projeto, use `docker compose stop`. Evite remover o volume se quiser preservar os cadastros.


