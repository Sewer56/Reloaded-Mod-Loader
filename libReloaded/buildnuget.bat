nuget pack -Properties Configuration=Release -IncludeReferencedProjects
echo ""
echo "Remember to Remove Fody & Fody.Costura in resultant package's nuspec and ReloadedAssembler executable."
echo ""
