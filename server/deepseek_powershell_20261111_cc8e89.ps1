$rootPath = (Get-Location).Path
$outputFile = Join-Path $rootPath "cleaned_contents_fixed.txt"

if (Test-Path $outputFile) { Remove-Item $outputFile -Force }

function Should-IncludeFile {
    param($file)
    $relativePath = $file.FullName.Substring($rootPath.Length).TrimStart('\')
    $ext = $file.Extension.ToLower()
    $name = $file.Name.ToLower()

    # Исключаем только WPF-проект client\AirGarap (включая .slnx и всё внутри)
    if ($relativePath -match '^client\\AirGarap(\\|$)') { return $false }

    # Исключаем служебные папки
    if ($relativePath -match '\\(bin|obj|\.git|\.vs|\.vscode|packages|TestResults|node_modules)\\') { return $false }
    if ($relativePath -match '^\.git\\') { return $false }
    if ($relativePath -match '^\.vs\\') { return $false }
    if ($relativePath -match '^\.vscode\\') { return $false }

    # Исключаем временные и сгенерированные файлы по расширению
    $excludeExt = @('.dll', '.exe', '.pdb', '.cache', '.baml', '.resources', '.user', '.suo', '.tmp', '.bak', '.orig', '.rej', '.log', '.txt', '.ps1', '.vbs')
    if ($excludeExt -contains $ext) { return $false }

    # Исключаем конкретные файлы (кроме README.md!)
    $excludeNames = @('conectionsettings.json', 'launchsettings.json', 'directory_contents.txt', 'generate_tree.ps1', '.gitignore', 'license')
    if ($excludeNames -contains $name) { return $false }

    # JSON — только appsettings.json и appsettings.Development.json
    if ($ext -eq '.json') {
        if ($name -ne 'appsettings.json' -and $name -ne 'appsettings.development.json') { return $false }
    }

    # Включаем нужные расширения (и README.md)
    $includeExt = @('.cs', '.xaml', '.sql', '.csproj', '.slnx', '.md')
    if ($includeExt -contains $ext) { return $true }
    if ($name -eq 'readme.md') { return $true }

    return $false
}

# 1. Дерево
"Directory Tree:" | Out-File $outputFile -Encoding UTF8

Get-ChildItem -Path $rootPath -Recurse -File | Where-Object { Should-IncludeFile $_ } | ForEach-Object {
    $_.FullName
} | Out-File $outputFile -Append -Encoding UTF8

"" | Out-File $outputFile -Append -Encoding UTF8

# 2. Содержимое
Get-ChildItem -Path $rootPath -Recurse -File | Where-Object { Should-IncludeFile $_ } | ForEach-Object {
    $file = $_
    $relativePath = $file.FullName.Substring($rootPath.Length).TrimStart('\')
    "File: $relativePath" | Out-File $outputFile -Append -Encoding UTF8
    "----------------" | Out-File $outputFile -Append -Encoding UTF8
    try {
        Get-Content -Path $file.FullName -Raw -ErrorAction Stop | Out-File $outputFile -Append -Encoding UTF8
    } catch {
        "Error reading file: $_" | Out-File $outputFile -Append -Encoding UTF8
    }
    " " | Out-File $outputFile -Append -Encoding UTF8
}

Write-Host "Done! Fixed file saved as: $outputFile"