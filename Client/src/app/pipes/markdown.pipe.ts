import { Pipe, PipeTransform, SecurityContext, inject } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";

@Pipe({
    name: "markdown",
    standalone: true
})
export class MarkdownPipe implements PipeTransform {
    domSanitizer = inject(DomSanitizer);
    async transform(content: string): Promise<string> {
        const { marked } = await import("marked");
        return (
            this.domSanitizer.sanitize(SecurityContext.HTML, content) || 
            ""
        )
    }
}