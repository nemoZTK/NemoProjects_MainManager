import os
from pathlib import Path

# piccola nota: si lo so che le parentesi sono apparentemente inutili, ma mi aiutano a collassare blocchi e a comprendere meglio cosa scrivo

LANGUAGE_STRUCTURE = { # QUI SONO DEFINITI I LINGUAGGI SUPPORTATI E LO SCHEMA DI CARTELLE DA CREARE
    "C": ["src", "include"],
    "Java": [],
    "C#": [],
    "Python": [],
} 

PROJECTS = ["Calculator","Converter","WallpapeRand","Hangman","GuessNumber2","Bonus"] # QUI DEFINITE LE CARTELLE DEI PROGETTINI

#================================================================= MAIN ==============================================
def print_banner():
#{
    """Stampa un banner da file bannerGen.txt se presente"""
    banner_path = Path(__file__).parent / "bannerGen.txt"
    if banner_path.exists():
        with banner_path.open("r", encoding="utf-8") as f:
            print(f.read())

    else:
        print("Benvenuto nel generatore di progetti NemoProjects!")

#}
# main, usa generate_projects e riceve l'input
def main():
# {
    print_banner()
    language = input("Inserisci il linguaggio (C/Java/C#/Python): ").strip()
    generate_projects(language)
# }


# flusso principale: genera i progetti usando create_base_folder, create_project_folders e create_base_readme
def generate_projects(language):
# {
    normalized_lang = normalize_language_name(language)
    base_folder, lang_cap = create_base_folder(normalized_lang)
    create_project_folders(base_folder, normalized_lang)
    create_base_readme(base_folder, lang_cap)
    print("Creazione dei progetti completata.")
# }


def normalize_language_name(language):
# {
    """Normalizza il nome del linguaggio (case insensitive, formattato come nel dict)"""
    for key in LANGUAGE_STRUCTURE.keys():
        if key.lower() == language.lower():
            return key
    return language  # fallback: restituisce il nome originale se non trovato
# }


#================================================================= BASE FOLDER ======================================

# crea la cartella base
def create_base_folder(language, base_path="."):
# {
    lang_cap = language.capitalize() if language.lower() != "c#" else "C#"
    project_path = Path(base_path) / f"NemoProjects_{lang_cap}"
    project_path.mkdir(parents=True, exist_ok=True)
    print(f"Creata cartella base: {project_path}")
    return project_path, lang_cap
# }

# crea il readme di base
def create_base_readme(base_path, language):
# {
    """Crea il readme.md nella cartella base con la lista dei progetti"""
    base_readme = base_path / "README.md"
    with base_readme.open("w", encoding="utf-8") as f:
    # {
        f.write(f"# Nemo Projects ({language})\n\n")
        f.write("Lista dei progetti:\n")
        for proj in PROJECTS:
            f.write(f"- {proj}\n")
    # }
# }

#================================================================= PROJECT FOLDERS =========================================

# crea le cartelle dei progettini e chiama create_project_readme
def create_project_folders(base_path, language):
# {
    """Crea le cartelle dei singoli progetti e le sottocartelle in base al linguaggio"""
    for proj in PROJECTS:
    # {
        proj_path = base_path / proj
        proj_path.mkdir(exist_ok=True)

        if language in LANGUAGE_STRUCTURE:
            for folder in LANGUAGE_STRUCTURE[language]:
                (proj_path / folder).mkdir(exist_ok=True)
        else:
            print(f"Attenzione: il linguaggio '{language}' non è supportato. Creo solo la cartella del progetto.")

        
        create_project_readme(proj_path, language)
    # }
# }


# crea il readme dei progettini 
def create_project_readme(proj_path, language):
# {
    """Crea il readme.md per un singolo progetto"""
    readme_path = proj_path / "README.md"
    with readme_path.open("w", encoding="utf-8") as f:
    # {
        f.write(f"# {proj_path.name}\n")
        f.write(f"Linguaggio: {language}\n\n")

        if language.lower() == "java":
        # {
            f.write("Comando per creare un progetto Java con Maven:\n")
            f.write(f"```\nmvn archetype:generate -DgroupId={proj_path.name} -DartifactId={proj_path.name} -DarchetypeArtifactId=maven-archetype-quickstart -DinteractiveMode=false\n```\n")
            f.write("Oppure usare Spring Initializr: https://start.spring.io/\n")
        # }
        elif language.lower() == "c#":
        # {
            f.write("Comandi per creare un progetto C#:\n\n")
            f.write(f"- Console App:\n```\ndotnet new console -n {proj_path.name}\n```\n")
            f.write(f"- Windows Forms App:\n```\ndotnet new winforms -n {proj_path.name}\n```\n")
            f.write(f"- Web API:\n```\ndotnet new webapi -n {proj_path.name}\n```\n")
        # } 
        elif language.lower() == "python":
        # {
            f.write("Comando per creare un progetto Python:\n")
            f.write(f"```python -m venv .venv && source .venv/bin/activate \n pip install --upgrade pip```\n")
        # }
    # } SE SI VUOLE AGGIUNGERE SUPPORTO PER ALTRI LINGUAGGI ANDREBBE MESSO QUI
# }



if __name__ == "__main__": main()

