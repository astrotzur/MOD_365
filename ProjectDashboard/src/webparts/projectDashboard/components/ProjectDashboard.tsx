import { PrimaryButton } from "@fluentui/react";
import { DetailsList, IColumn, SelectionMode } from "@fluentui/react";
import { SPHttpClient } from "@microsoft/sp-http";
import { Dropdown, IDropdownOption } from "@fluentui/react";
import { sp } from "@pnp/sp";
import "@pnp/sp/webs";
import "@pnp/sp/lists";
import "@pnp/sp/items";

import * as React from 'react';
import styles from './ProjectDashboard.module.scss';
import { IProjectDashboardProps } from './IProjectDashboardProps';
import { escape } from '@microsoft/sp-lodash-subset';

import { IProject } from "./IProject";

interface IProjectDashboardState {
  projects: IProject[];
  statusOptions: IDropdownOption[];
  selectedStatus: string;
}

export default class ProjectDashboard extends React.Component<IProjectDashboardProps, IProjectDashboardState> {

  public constructor(props: IProjectDashboardProps) {
    super(props);
    this.state = {
      projects: [],
      statusOptions: [{ key: "All", text: "All" }],
      selectedStatus: "All"
    };
  }

  private async loadProjects(): Promise<void> {
    try {
      const items = await sp.web.lists
        .getByTitle("Projects")
        .items
        .select("Id", "Title", "Status", "Owner/Title", "StartDate")
        .expand("Owner")
        .get();

      const mapped: IProject[] = items.map(item => ({
        Id: item.Id,
        Title: item.Title,
        Status: item.Status,
        OwnerTitle: item.Owner ? item.Owner.Title : "",
        StartDate: item.StartDate
      }));

      this.setState({ projects: mapped });
    } catch (error) {
      console.error("Error loading projects", error);
    }
  }

  private async loadStatusChoices(): Promise<void> {
    try {
      const response = await this.props.context.spHttpClient.get(
        `${this.props.context.pageContext.web.absoluteUrl}/_api/web/lists/getbytitle('Projects')/fields?$filter=EntityPropertyName eq 'Status'`,
        SPHttpClient.configurations.v1
      );

      if (!response.ok) {
        throw new Error(`Error loading fields: ${response.statusText}`);
      }

      const json = await response.json();
      const field = json.value && json.value[0];
      console.log("Retrieved field:", field);

      if (field && field.Choices) {
        const dynamicOptions = field.Choices.map(choice => ({
          key: choice,
          text: choice
        }));

        this.setState({
          statusOptions: [{ key: "All", text: "All" }, ...dynamicOptions]
        });
      } else {
        console.warn("Status field not found or has no choices", field);
      }
    } catch (err) {
      console.error("Error loading status choices", err);
    }
  }

  public async componentDidMount(): Promise<void> {
    sp.setup({
      spfxContext: this.props.context
    });

    await this.loadStatusChoices();
    await this.loadProjects();
  }

  private onStatusFilterChanged(option: IDropdownOption): void {
    this.setState({ selectedStatus: option.key as string });
  }

  private async finishProject(id: number): Promise<void> {
    try {
      await sp.web.lists
        .getByTitle("Projects")
        .items
        .getById(id)
        .update({
          Status: "Closed"
        });

      console.log(`Project ${id} closed successfully`);
      await this.loadProjects(); // refresh
    } catch (err) {
      console.error("Error closing project", err);
    }
  }



  public render(): React.ReactElement<IProjectDashboardProps> {
    const columns: IColumn[] = [
      { key: 'title', name: 'Title', fieldName: 'Title', minWidth: 100, isResizable: true },
      { key: 'status', name: 'Status', fieldName: 'Status', minWidth: 100, isResizable: true },
      { key: 'owner', name: 'Owner', fieldName: 'OwnerTitle', minWidth: 100, isResizable: true },
      { key: 'startDate', name: 'Start Date', fieldName: 'StartDate', minWidth: 100, isResizable: true },
      {
        key: 'actions',
        name: 'Actions',
        minWidth: 100,
        onRender: (item: IProject) => {
          return item.Status !== "Closed" ? (
            <PrimaryButton
              text="Finish Project"
              onClick={() => this.finishProject(item.Id)}
            />
          ) : null;
        }
      }
    ];

    return (
      <div>
        <Dropdown
          placeholder="Filter by status"
          label="Project Status"
          options={this.state.statusOptions}
          selectedKey={this.state.selectedStatus}
          onChange={(_, option) => this.onStatusFilterChanged(option)}
        />
        <DetailsList
          items={this.state.projects.filter(
            (proj) =>
              this.state.selectedStatus === "All" ||
              proj.Status === this.state.selectedStatus
          )}
          columns={columns}
          selectionMode={SelectionMode.none}
        />
      </div>
    );
  }
}