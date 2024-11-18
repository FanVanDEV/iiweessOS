# iiweessOS — GUI эмулятор терминала, выполненный на C# WinForms

Эмулятор командной строки с графическим интерфейсом, который повторяет UNIX shell.
## Поддерживаемые команды

### 1. `cal`
Команда для отображения календаря.

### 2. `date`
Команда для отображения текущей даты и времени.

### 3. `ls`
Команда для отображения содержимого директории.

### 4. `cd`
Команда для изменения текущей директории.

### 5. `rm`
Команда для удаления файлов и директорий.

### 6. `clear`
Команда для очистки экрана.

### 7. `exit`
Команда для выхода из эмулятора.

## Конфигурация

Эмулятор использует файл конфигурации config.yml, где задаются параметры для виртуальной файловой системы и имя пользователя.

Пример `config.yml`:
```yaml
user: "guest"
filesystem: "fs.tar"
```

# Установка и запуск
1. Клонируйте репозиторий:

```bash
git clone https://github.com/FanVanDEV/iiweessOS.git
cd iiweessOS
```
2. Откройте проект в Visual Studio и убедитесь, что установлены все зависимости.

3. Сконфигурируйте файл `config.yml` с необходимыми параметрами (путь к виртуальной файловой системе `.tar` и имя пользователя).

4. Запустите приложение из Visual Studio.

5. После запуска эмулятора откроется окно, где можно вводить команды, как в настоящем терминале!

🥳 Вы великолепны!
